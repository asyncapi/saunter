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

            var asyncApiTypes = GetAsyncApiTypes(options, documentName);

            var apiNamePair = options.NamedApis.FirstOrDefault(c => c.Value.Id == documentName);

            var clone = _cloner.CloneProtype(apiNamePair.Value ?? options.AsyncApi);

            if (string.IsNullOrWhiteSpace(clone.DefaultContentType))
            {
                clone.DefaultContentType = "application/json";
            }

            var channelItems = Enumerable.Concat(
                GenerateChannelsFromMethods(clone.Components, options, asyncApiTypes),
                GenerateChannelsFromClasses(clone.Components, options, asyncApiTypes));

            foreach (var item in channelItems)
            {
                if (!clone.Channels.TryAdd(item.Key, item.Value))
                {
                    clone.Channels[item.Key] = _channelUnion.Union(
                        clone.Channels[item.Key],
                        item.Value);
                }
            }

            var filterContext = new DocumentFilterContext(asyncApiTypes);

            foreach (var filterType in options.DocumentFilters)
            {
                var filter = (IDocumentFilter)_serviceProvider.GetRequiredService(filterType);
                filter?.Apply(clone, filterContext);
            }

            return clone;
        }

        private IEnumerable<KeyValuePair<string, AsyncApiChannel>> GenerateChannelsFromMethods(AsyncApiComponents components, AsyncApiOptions options, TypeInfo[] asyncApiTypes)
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
                    Parameters = GetChannelParametersFromAttributes(components, item.Method),
                    Publish = GenerateOperationFromMethod(components, item.Method, OperationType.Publish, options),
                    Subscribe = GenerateOperationFromMethod(components, item.Method, OperationType.Subscribe, options),
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

                ApplyChannelFilters(options, item.Method, item.Channel, channelItem);

                yield return new(item.Channel.Name, channelItem);
            }
        }

        private IEnumerable<KeyValuePair<string, AsyncApiChannel>> GenerateChannelsFromClasses(AsyncApiComponents components, AsyncApiOptions options, TypeInfo[] asyncApiTypes)
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
                    Parameters = GetChannelParametersFromAttributes(components, item.Type),
                    Publish = GenerateOperationFromClass(components, item.Type, OperationType.Publish),
                    Subscribe = GenerateOperationFromClass(components, item.Type, OperationType.Subscribe),
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

                ApplyChannelFilters(options, item.Type, item.Channel, channelItem);

                yield return new(item.Channel.Name, channelItem);
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

        private IDictionary<string, AsyncApiParameter> GetChannelParametersFromAttributes(AsyncApiComponents components, MemberInfo memberInfo)
        {
            var attributes = memberInfo.GetCustomAttributes<ChannelParameterAttribute>();
            var parameters = new Dictionary<string, AsyncApiParameter>(attributes.Count());

            foreach (var attribute in attributes)
            {
                var parameterId = attribute.Name;

                if (!components.Parameters.ContainsKey(parameterId))
                {
                    var schema = GetAsyncApiSchema(components, (TypeInfo?)attribute.Type);

                    var parameter = new AsyncApiParameter
                    {
                        Description = attribute.Description,
                        Location = attribute.Location,
                        Schema = schema,
                    };

                    components.Parameters.Add(parameterId, parameter);
                }

                parameters.Add(parameterId, new()
                {
                    Reference = new()
                    {
                        Id = parameterId,
                        Type = ReferenceType.Parameter,
                    }
                });
            }

            return parameters;
        }

        private AsyncApiOperation? GenerateOperationFromMethod(AsyncApiComponents components, MethodInfo method, OperationType operationType, AsyncApiOptions options)
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
                operation.Message = GenerateMessageFromAttributes(components, messageAttributes);
            }
            else if (operationAttribute.MessagePayloadType is not null)
            {
                operation.Message = GenerateMessageFromType(components, operationAttribute.MessagePayloadType.GetTypeInfo());
            }

            ApplyOprationFilters(method, options, operationAttribute, operation);

            return operation;
        }

        private AsyncApiOperation? GenerateOperationFromClass(AsyncApiComponents components, TypeInfo type, OperationType operationType)
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
                var message = GenerateMessageFromAttribute(components, attribute);

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

        private void ApplyOprationFilters(MethodInfo method, AsyncApiOptions options, OperationAttribute operationAttribute, AsyncApiOperation operation)
        {
            var filterContext = new OperationFilterContext(method, operationAttribute);

            foreach (var filterType in options.OperationFilters)
            {
                var filter = (IOperationFilter)_serviceProvider.GetRequiredService(filterType);
                filter?.Apply(operation, filterContext);
            }
        }

        private List<AsyncApiMessage> GenerateMessageFromAttributes(AsyncApiComponents components, IEnumerable<MessageAttribute> messageAttributes)
        {
            List<AsyncApiMessage> messages = new();

            if (messageAttributes.Count() == 1)
            {
                var message = GenerateMessageFromAttribute(components, messageAttributes.First());

                if (message is not null)
                {
                    messages.Add(message);
                }

                return messages;
            }

            foreach (var attribute in messageAttributes)
            {
                var message = GenerateMessageFromAttribute(components, attribute);

                if (message != null)
                {
                    messages.Add(message);
                }
            }

            return messages;
        }

        private List<AsyncApiMessage> GenerateMessageFromType(AsyncApiComponents components, TypeInfo payloadType)
        {
            var asyncApiSchema = GetAsyncApiSchema(components, payloadType);

            var messageId = asyncApiSchema?.Title;

            if (messageId is null)
            {
                return new();
            }

            if (!components.Messages.ContainsKey(messageId))
            {
                var message = new AsyncApiMessage
                {
                    Payload = asyncApiSchema,
                    MessageId = messageId,
                    Name = messageId,
                    Title = messageId,
                };

                components.Messages.Add(messageId, message);
            }

            return new()
            {
                new()
                {
                    Reference = new()
                    {
                        Id = messageId,
                        Type = ReferenceType.Message,
                    }
                }
            };
        }

        private AsyncApiMessage? GenerateMessageFromAttribute(AsyncApiComponents components, MessageAttribute messageAttribute)
        {
            if (messageAttribute?.PayloadType == null)
            {
                return null;
            }

            var bodySchema = GetAsyncApiSchema(components, (TypeInfo)messageAttribute.PayloadType);

            var messageId = messageAttribute.MessageId ?? bodySchema?.Title ?? messageAttribute.PayloadType.Name;

            if (!components.Messages.ContainsKey(messageId))
            {
                var tags = messageAttribute.Tags?
                    .Select(x => new AsyncApiTag { Name = x })
                    .ToList() ?? new List<AsyncApiTag>();

                var headersSchema = GetAsyncApiSchema(components, (TypeInfo?)messageAttribute.HeadersType);

                var message = new AsyncApiMessage
                {
                    MessageId = messageId,
                    Title = messageAttribute.Title ?? bodySchema?.Title ?? messageAttribute.PayloadType.Name,
                    Name = messageAttribute.Name ?? bodySchema?.Title ?? messageAttribute.PayloadType.Name,
                    Summary = messageAttribute.Summary,
                    Description = messageAttribute.Description,
                    Tags = tags,
                    Payload = bodySchema,
                    Headers = headersSchema,
                    Bindings = new()
                    {
                        Reference = new()
                        {
                            Id = messageAttribute.BindingsRef,
                            Type = ReferenceType.MessageBindings,
                        },
                    },
                };

                components.Messages.Add(message.MessageId, message);
            }

            return new()
            {
                Reference = new()
                {
                    Id = messageId,
                    Type = ReferenceType.Message,
                }
            };
        }

        private AsyncApiSchema? GetAsyncApiSchema(AsyncApiComponents components, TypeInfo? payloadType)
        {
            var generatedSchemas = _schemaGenerator.Generate(payloadType);

            if (generatedSchemas is null)
            {
                return null;
            }

            foreach (var asyncApiSchema in generatedSchemas.Value.All)
            {
                var key = asyncApiSchema.Title;

                if (!components.Schemas.ContainsKey(key))
                {
                    components.Schemas[key] = asyncApiSchema;
                }
            }

            return new()
            {
                Title = generatedSchemas.Value.Root.Title,
                Reference = new()
                {
                    Id = generatedSchemas.Value.Root.Title,
                    Type = ReferenceType.Schema,
                }
            };
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
