using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Namotion.Reflection;
using Saunter.AttributeProvider.Attributes;
using Saunter.Options;
using Saunter.Options.Filters;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.AttributeProvider
{
    internal class AttributeDocumentProvider : IAsyncApiDocumentProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAsyncApiSchemaGenerator _schemaGenerator;
        private readonly IAsyncApiChannelUnion _channelUnion;
        private readonly IAsyncApiDocumentCloner _cloner;

        public AttributeDocumentProvider(IServiceProvider serviceProvider, IAsyncApiSchemaGenerator schemaGenerator, IAsyncApiChannelUnion channelUnion, IAsyncApiDocumentCloner cloner)
        {
            _serviceProvider = serviceProvider;
            _schemaGenerator = schemaGenerator;
            _channelUnion = channelUnion;
            _cloner = cloner;
        }

        public AsyncApiDocument GetDocument(string? documentName, AsyncApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var apiNamePair = options.NamedApis
                .FirstOrDefault(c => c.Value.Id == documentName);

            var asyncApiTypes = GetAsyncApiTypes(options, apiNamePair.Key);

            var clone = _cloner.CloneProtype(apiNamePair.Value ?? options.AsyncApi);

            GenerateChannelsFromMethods(clone, options, asyncApiTypes);
            GenerateChannelsFromClasses(clone, options, asyncApiTypes);

            var filterContext = new DocumentFilterContext(asyncApiTypes);

            foreach (var filterType in options.DocumentFilters)
            {
                var filter = (IDocumentFilter)_serviceProvider.GetRequiredService(filterType);
                filter?.Apply(clone, filterContext);
            }

            return clone;
        }

        private void GenerateChannelsFromMethods(AsyncApiDocument document, AsyncApiOptions options, TypeInfo[] asyncApiTypes)
        {
            var methodsWithChannelAttribute = asyncApiTypes
                .SelectMany(type => type.DeclaredMethods)
                .Select(method => new
                {
                    Channel = method.GetCustomAttribute<ChannelAttribute>(),
                    Method = method,
                })
                .Where(mc => mc.Channel != null);

            foreach (var item in methodsWithChannelAttribute)
            {
                if (item.Channel == null)
                {
                    continue;
                }

                var channelItem = new AsyncApiChannel
                {
                    Servers = item.Channel.Servers?.ToList(),
                    Description = item.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(item.Method),
                    Publish = GenerateOperationFromMethod(item.Method, OperationType.Publish, options),
                    Subscribe = GenerateOperationFromMethod(item.Method, OperationType.Subscribe, options),
                    Bindings = item.Channel.BindingsRef != null
                        ? new()
                        {
                            Reference = new()
                            {
                                Id = item.Channel.BindingsRef,
                                Type = ReferenceType.ChannelBindings,
                            }
                        }
                        : null,
                };

                if (!document.Channels.TryAdd(item.Channel.Name, channelItem))
                {
                    document.Channels[item.Channel.Name] = _channelUnion.Union(
                        document.Channels[item.Channel.Name],
                        channelItem);
                }

                ApplyChannelFilters(options, item.Method, item.Channel, channelItem);
            }
        }

        private void GenerateChannelsFromClasses(AsyncApiDocument document, AsyncApiOptions options, TypeInfo[] asyncApiTypes)
        {
            var classesWithChannelAttribute = asyncApiTypes
                .Select(type => new
                {
                    Channel = type.GetCustomAttribute<ChannelAttribute>(),
                    Type = type,
                })
                .Where(cc => cc.Channel != null);

            foreach (var item in classesWithChannelAttribute)
            {
                if (item.Channel == null)
                {
                    continue;
                }

                var channelItem = new AsyncApiChannel
                {
                    Description = item.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(item.Type),
                    Publish = GenerateOperationFromClass(item.Type, OperationType.Publish),
                    Subscribe = GenerateOperationFromClass(item.Type, OperationType.Subscribe),
                    Servers = item.Channel.Servers?.ToList(),
                    Bindings = item.Channel.BindingsRef != null
                        ? new()
                        {
                            Reference = new()
                            {
                                Id = item.Channel.BindingsRef,
                                Type = ReferenceType.ChannelBindings,
                            }
                        }
                        : null,
                };

                if (!document.Channels.TryAdd(item.Channel.Name, channelItem))
                {
                    document.Channels[item.Channel.Name] = _channelUnion.Union(
                        document.Channels[item.Channel.Name],
                        channelItem);
                }

                ApplyChannelFilters(options, item.Type, item.Channel, channelItem);
            }
        }

        private void ApplyChannelFilters(AsyncApiOptions options, MemberInfo member, ChannelAttribute channel, AsyncApiChannel channelItem)
        {
            var context = new ChannelFilterContext(member, channel);

            foreach (var filterType in options.ChannelFilters)
            {
                var filter = (IChannelFilter)_serviceProvider.GetRequiredService(filterType);
                filter.Apply(channelItem, context);
            }
        }

        private IDictionary<string, AsyncApiParameter> GetChannelParametersFromAttributes(MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes<ChannelParameterAttribute>();
            var parameters = new Dictionary<string, AsyncApiParameter>();

            if (attributes.Any())
            {
                foreach (var attribute in attributes)
                {
                    var parameter = new AsyncApiParameter
                    {
                        Description = attribute.Description,
                        Location = attribute.Location,
                        Schema = _schemaGenerator.Generate(attribute.Type),
                    };

                    parameters.Add(attribute.Name, parameter);
                }
            }

            return parameters;
        }

        private AsyncApiOperation? GenerateOperationFromMethod(MethodInfo method, OperationType operationType, AsyncApiOptions options)
        {
            var operationAttribute = GetOperationAttribute(method, operationType);

            if (operationAttribute == null)
            {
                return null;
            }

            var messageAttributes = method.GetCustomAttributes<MessageAttribute>();

            var tags = operationAttribute
                .Tags?
                .Select(x => new AsyncApiTag { Name = x })
                .ToList() ?? new List<AsyncApiTag>();

            var description = operationAttribute.Description ??
                (method.GetXmlDocsRemarks() != string.Empty
                    ? method.GetXmlDocsRemarks()
                    : null);

            var bindings = operationAttribute.BindingsRef != null
                ? new AsyncApiBindings<LEGO.AsyncAPI.Models.Interfaces.IOperationBinding>()
                {
                    Reference = new()
                    {
                        Id = operationAttribute.BindingsRef,
                        Type = ReferenceType.OperationBindings,
                    }
                }
                : null;

            var operation = new AsyncApiOperation
            {
                Tags = tags,
                Description = description,
                Message = new List<AsyncApiMessage>(),
                OperationId = operationAttribute.OperationId ?? method.Name,
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Bindings = bindings,
            };

            if (messageAttributes.Any())
            {
                operation.Message = GenerateMessageFromAttributes(messageAttributes);
            }
            else if (operationAttribute.MessagePayloadType is not null)
            {
                operation.Message = GenerateMessageFromType(operationAttribute.MessagePayloadType.GetTypeInfo());
            }

            var filterContext = new OperationFilterContext(method, operationAttribute);
            foreach (var filterType in options.OperationFilters)
            {
                var filter = (IOperationFilter)_serviceProvider.GetRequiredService(filterType);
                filter?.Apply(operation, filterContext);
            }

            return operation;
        }

        private AsyncApiOperation? GenerateOperationFromClass(TypeInfo type, OperationType operationType)
        {
            var operationAttribute = GetOperationAttribute(type, operationType);

            if (operationAttribute == null)
            {
                return null;
            }

            var messages = new List<AsyncApiMessage>();

            var tags = operationAttribute
                .Tags?
                .Select(x => new AsyncApiTag() { Name = x })
                .ToList() ?? new List<AsyncApiTag>();

            var operation = new AsyncApiOperation
            {
                Tags = tags,
                Message = messages,
                OperationId = operationAttribute.OperationId ?? type.Name,
                Summary = operationAttribute.Summary ?? type.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (type.GetXmlDocsRemarks() != "" ? type.GetXmlDocsRemarks() : null),
                Bindings = operationAttribute.BindingsRef != null
                    ? new()
                    {
                        Reference = new()
                        {
                            Id = operationAttribute.BindingsRef,
                            Type = ReferenceType.OperationBindings
                        }
                    }
                    : null,
            };

            var attributes = type
                .DeclaredMethods
                .Select(m => new
                {
                    MessageAttributes = m.GetCustomAttributes<MessageAttribute>(),
                    Method = m,
                })
                .Where(x => x.MessageAttributes.Any())
                .SelectMany(x => x.MessageAttributes);

            foreach (var attribute in attributes)
            {
                var message = GenerateMessageFromAttribute(attribute);

                if (message != null)
                {
                    messages.Add(message);
                }
            }

            return operation;
        }

        private static OperationAttribute? GetOperationAttribute(MemberInfo typeOrMethod, OperationType operationType)
        {
            return operationType switch
            {
                OperationType.Publish => typeOrMethod.GetCustomAttribute<PublishOperationAttribute>(),
                OperationType.Subscribe => typeOrMethod.GetCustomAttribute<SubscribeOperationAttribute>(),
                _ => null,
            };
        }

        private List<AsyncApiMessage> GenerateMessageFromAttributes(IEnumerable<MessageAttribute> messageAttributes)
        {
            List<AsyncApiMessage> messages = new();

            if (messageAttributes.Count() == 1)
            {
                var message = GenerateMessageFromAttribute(messageAttributes.First());

                if (message is not null)
                {
                    messages.Add(message);
                }

                return messages;
            }

            foreach (var attribute in messageAttributes)
            {
                var message = GenerateMessageFromAttribute(attribute);

                if (message != null)
                {
                    messages.Add(message);
                }
            }

            return messages;
        }

        private List<AsyncApiMessage> GenerateMessageFromType(TypeInfo payloadType)
        {
            var message = new AsyncApiMessage
            {
                Payload = _schemaGenerator.Generate(payloadType),
            };

            message.MessageId = message.Payload?.Title;
            message.Name = message.Payload?.Title;

            return new() { message };
        }

        private AsyncApiMessage? GenerateMessageFromAttribute(MessageAttribute messageAttribute)
        {
            if (messageAttribute?.PayloadType == null)
            {
                return null;
            }

            var tags = messageAttribute.Tags?
                .Select(x => new AsyncApiTag { Name = x })
                .ToList() ?? new List<AsyncApiTag>();

            var asyncApiSchema = _schemaGenerator.Generate(messageAttribute.PayloadType);

            var message = new AsyncApiMessage
            {
                MessageId = messageAttribute.MessageId ?? asyncApiSchema?.Title ?? messageAttribute.PayloadType.Name,
                Title = messageAttribute.Title,
                Summary = messageAttribute.Summary,
                Description = messageAttribute.Description,
                Tags = tags,
                Payload = asyncApiSchema,
                Headers = _schemaGenerator.Generate(messageAttribute.HeadersType),
                Name = messageAttribute.Name ?? asyncApiSchema?.Title ?? messageAttribute.PayloadType.Name,
                Bindings = new()
                {
                    Reference = new()
                    {
                        Id = messageAttribute.BindingsRef,
                        Type = ReferenceType.MessageBindings,
                    },
                },
            };

            return message;
        }

        private static TypeInfo[] GetAsyncApiTypes(AsyncApiOptions options, string? apiName)
        {
            var asyncApiTypes = options
                .AsyncApiSchemaTypes
                    .Where(t => t.GetCustomAttribute<AsyncApiAttribute>()?.DocumentName == apiName)
                .ToArray();

            return asyncApiTypes;
        }
    }
}
