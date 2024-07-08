using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LEGO.AsyncAPI.Models;
using Namotion.Reflection;
using Saunter.AttributeProvider.Attributes;
using Saunter.Options;
using Saunter.SharedKernel;
using Saunter.SharedKernel.Interfaces;

namespace Saunter.AttributeProvider
{
    internal class AttributeDocumentProvider : IAsyncApiDocumentProvider
    {
        private readonly IAsyncApiSchemaGenerator _schemaGenerator;
        private readonly IAsyncApiDocumentCloner _cloner;

        public AttributeDocumentProvider(IAsyncApiSchemaGenerator asyncApiSchemaGenerator, IAsyncApiDocumentCloner cloner)
        {
            _schemaGenerator = asyncApiSchemaGenerator;
            _cloner = cloner;
        }

        public AsyncApiDocument GetDocument(AsyncApiOptions options, AsyncApiDocument prototype)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var apiNamePair = options.NamedApis
                .FirstOrDefault(c => c.Value.Id == prototype.Id);

            if (apiNamePair.Value is null)
            {
                throw new InvalidOperationException($"Not found document with name: {prototype.Id}");
            }

            var asyncApiTypes = GetAsyncApiTypes(options, apiNamePair.Key);

            var clone = _cloner.CloneProtype(prototype);

            GenerateChannelsFromMethods(clone, options, asyncApiTypes);
            GenerateChannelsFromClasses(clone, options, asyncApiTypes);

            // TODO: RESTORE
            // var filterContext = new DocumentFilterContext(asyncApiTypes, schemaResolver, generator);
            // foreach (var filterType in options.DocumentFilters)
            // {
            //     var filter = (IDocumentFilter)serviceProvider.GetRequiredService(filterType);
            //     filter?.Apply(asyncApiSchema, filterContext);
            // }

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

                document.Channels.Add(item.Channel.Name, channelItem);

                // TODO: RESTORE
                //var context = new ChannelItemFilterContext(mc.Method, schemaResolver, jsonSchemaGenerator, mc.Channel);
                //foreach (var filterType in options.ChannelItemFilters)
                //{
                //    var filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                //    filter.Apply(channelItem, context);
                //}
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
                if (item.Channel == null) continue;

                var channelItem = new AsyncApiChannel
                {
                    Description = item.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(item.Type),
                    Publish = GenerateOperationFromClass(item.Type, OperationType.Publish, options),
                    Subscribe = GenerateOperationFromClass(item.Type, OperationType.Subscribe, options),
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

                document.Channels.Add(item.Channel.Name, channelItem);

                // TODO: RESTORE
                //var context = new ChannelItemFilterContext(item.Type, schemaResolver, jsonSchemaGenerator, item.Channel);
                //foreach (var filterType in options.ChannelItemFilters)
                //{
                //    var filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                //    filter.Apply(channelItem, context);
                //}
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

            var message = messageAttributes.Any()
                ? GenerateMessageFromAttributes(messageAttributes)
                : GenerateMessageFromType(operationAttribute.MessagePayloadType.GetTypeInfo());

            if (message is not null)
            {
                operation.Message.Add(message);
            }

            // TODO: RESTORE
            // var filterContext = new OperationFilterContext(method, schemaResolver, jsonSchemaGenerator, operationAttribute);

            // foreach (var filterType in options.OperationFilters)
            //{
            //    var filter = (IOperationFilter)serviceProvider.GetRequiredService(filterType);
            //    filter?.Apply(operation, filterContext);
            //}

            return operation;
        }

        private AsyncApiOperation? GenerateOperationFromClass(TypeInfo type, OperationType operationType, AsyncApiOptions options)
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

        private AsyncApiMessage? GenerateMessageFromAttributes(IEnumerable<MessageAttribute> messageAttributes)
        {
            if (messageAttributes.Count() == 1)
            {
                return GenerateMessageFromAttribute(messageAttributes.First());
            }

            var messages = new AsyncApiMessage();

            foreach (var attribute in messageAttributes)
            {
                var message = GenerateMessageFromAttribute(attribute);

                if (message != null)
                {
                    // TODO: OneOf? AllOf?
                    messages.Payload.AnyOf.Add(message.Payload);
                }
            }

            return messages;
        }

        private AsyncApiMessage? GenerateMessageFromType(TypeInfo payloadType)
        {
            if (payloadType == null)
            {
                return null;
            }

            var message = new AsyncApiMessage
            {
                Payload = _schemaGenerator.Generate(payloadType),
            };

            message.MessageId = message.Payload?.Title;
            message.Name = message.Payload?.Title;

            return message;
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

            var message = new AsyncApiMessage
            {
                MessageId = messageAttribute.MessageId,
                Title = messageAttribute.Title,
                Summary = messageAttribute.Summary,
                Description = messageAttribute.Description,
                Tags = tags,
                Payload = _schemaGenerator.Generate(messageAttribute.PayloadType),
                Headers = _schemaGenerator.Generate(messageAttribute.HeadersType),
                Name = messageAttribute.Name ?? messageAttribute.PayloadType.Name,
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

        /// <summary>
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies <see cref="AsyncApiOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private static TypeInfo[] GetAsyncApiTypes(AsyncApiOptions options, string apiName)
        {
            var assembliesToScan = options
                .AssemblyMarkerTypes
                .Select(t => t.Assembly)
                .Distinct();

            var asyncApiTypes = assembliesToScan
                .SelectMany(a => a.DefinedTypes
                    .Where(t => t.GetCustomAttribute<AsyncApiAttribute>()?.DocumentName == apiName))
                .ToArray();

            return asyncApiTypes;
        }
    }
}
