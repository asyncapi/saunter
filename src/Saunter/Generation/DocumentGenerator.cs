using Namotion.Reflection;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.Utils;

namespace Saunter.Generation
{
    public class DocumentGenerator : IDocumentGenerator
    {
        public DocumentGenerator()
        {
        }

        public AsyncApiSchema.v2.AsyncApiDocument GenerateDocument(TypeInfo[] asyncApiTypes, AsyncApiOptions options, AsyncApiDocument prototype, IServiceProvider serviceProvider)
        {
            var asyncApiSchema = prototype.Clone();

            var schemaResolver = new AsyncApiSchemaResolver(asyncApiSchema, options.SchemaOptions);

            var generator = new JsonSchemaGenerator(options.SchemaOptions);
            asyncApiSchema.Channels = GenerateChannels(asyncApiTypes, schemaResolver, options, generator, serviceProvider);
            
            var filterContext = new DocumentFilterContext(asyncApiTypes, schemaResolver, generator);
            foreach (var filterType in options.DocumentFilters)
            {
                var filter = (IDocumentFilter)serviceProvider.GetRequiredService(filterType);
                filter?.Apply(asyncApiSchema, filterContext);
            }

            return asyncApiSchema;
        }

        /// <summary>
        /// Generate the Channels section of an AsyncApi schema.
        /// </summary>
        private static IDictionary<string, ChannelItem> GenerateChannels(TypeInfo[] asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
        {
            var channels = new Dictionary<string, ChannelItem>();
            
            channels.AddRange(GenerateChannelsFromMethods(asyncApiTypes, schemaResolver, options, jsonSchemaGenerator, serviceProvider));
            channels.AddRange(GenerateChannelsFromClasses(asyncApiTypes, schemaResolver, options, jsonSchemaGenerator, serviceProvider));
            return channels;
        }

        /// <summary>
        /// Generate the Channels section of the AsyncApi schema from the
        /// <see cref="ChannelAttribute"/> on methods.
        /// </summary>
        private static IDictionary<string, ChannelItem> GenerateChannelsFromMethods(IEnumerable<TypeInfo> asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
        {
            var channels = new Dictionary<string, ChannelItem>();

            var methodsWithChannelAttribute = asyncApiTypes
                .SelectMany(type => type.DeclaredMethods)
                .Select(method => new
                {
                    Channel = method.GetCustomAttribute<ChannelAttribute>(),
                    Method = method,
                })
                .Where(mc => mc.Channel != null);

            foreach (var mc in methodsWithChannelAttribute)
            {
                if (mc.Channel == null) continue;
                
                var channelItem = new ChannelItem
                {
                    Description = mc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(mc.Method, schemaResolver, jsonSchemaGenerator),
                    Publish = GenerateOperationFromMethod(mc.Method, schemaResolver, OperationType.Publish, options, jsonSchemaGenerator, serviceProvider),
                    Subscribe = GenerateOperationFromMethod(mc.Method, schemaResolver, OperationType.Subscribe, options, jsonSchemaGenerator, serviceProvider),
                    Bindings = mc.Channel.BindingsRef != null ? new ChannelBindingsReference(mc.Channel.BindingsRef) : null,
                    Servers = mc.Channel.Servers?.ToList(),
                }; 
                channels.AddOrAppend(mc.Channel.Name, channelItem);
                
                var context = new ChannelItemFilterContext(mc.Method, schemaResolver, jsonSchemaGenerator, mc.Channel);
                foreach (var filterType in options.ChannelItemFilters)
                {
                    var filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }

            return channels;
        }

        /// <summary>
        /// Generate the Channels section of the AsyncApi schema from the
        /// <see cref="ChannelAttribute"/> on classes.
        /// </summary>
        private static IDictionary<string, ChannelItem> GenerateChannelsFromClasses(IEnumerable<TypeInfo> asyncApiTypes, AsyncApiSchemaResolver schemaResolver, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
        {
            var channels = new Dictionary<string, ChannelItem>();

            var classesWithChannelAttribute = asyncApiTypes
                .Select(type => new
                {
                    Channel = type.GetCustomAttribute<ChannelAttribute>(),
                    Type = type,
                })
                .Where(cc => cc.Channel != null);

            foreach (var cc in classesWithChannelAttribute)
            {
                if (cc.Channel == null) continue;
                
                var channelItem = new ChannelItem
                {
                    Description = cc.Channel.Description,
                    Parameters = GetChannelParametersFromAttributes(cc.Type, schemaResolver, jsonSchemaGenerator),
                    Publish = GenerateOperationFromClass(cc.Type, schemaResolver, OperationType.Publish, jsonSchemaGenerator),
                    Subscribe = GenerateOperationFromClass(cc.Type, schemaResolver, OperationType.Subscribe, jsonSchemaGenerator),
                    Bindings = cc.Channel.BindingsRef != null ? new ChannelBindingsReference(cc.Channel.BindingsRef) : null,
                    Servers = cc.Channel.Servers?.ToList(),
                };
                
                channels.AddOrAppend(cc.Channel.Name, channelItem);
                
                var context = new ChannelItemFilterContext(cc.Type, schemaResolver, jsonSchemaGenerator, cc.Channel);
                foreach (var filterType in options.ChannelItemFilters)
                {
                    var filter = (IChannelItemFilter)serviceProvider.GetRequiredService(filterType);
                    filter.Apply(channelItem, context);
                }
            }

            return channels;
        }

        /// <summary>
        /// Generate the an operation of an AsyncApi Channel for the given method.
        /// </summary>
        private static Operation GenerateOperationFromMethod(MethodInfo method, AsyncApiSchemaResolver schemaResolver, OperationType operationType, AsyncApiOptions options, JsonSchemaGenerator jsonSchemaGenerator, IServiceProvider serviceProvider)
        {
            var operationAttribute = GetOperationAttribute(method, operationType);
            if (operationAttribute == null)
            {
                return null;
            }

            IEnumerable<MessageAttribute> messageAttributes = method.GetCustomAttributes<MessageAttribute>();
            var message = messageAttributes.Any()
                ? GenerateMessageFromAttributes(messageAttributes, schemaResolver, jsonSchemaGenerator)
                : GenerateMessageFromType(operationAttribute.MessagePayloadType, schemaResolver, jsonSchemaGenerator);
            
            var operation = new Operation
            {
                OperationId = operationAttribute.OperationId ?? method.Name,
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (method.GetXmlDocsRemarks() != "" ? method.GetXmlDocsRemarks() : null),
                Message = message,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
            };

            var filterContext = new OperationFilterContext(method, schemaResolver, jsonSchemaGenerator, operationAttribute);
            foreach (var filterType in options.OperationFilters)
            {
                var filter = (IOperationFilter)serviceProvider.GetRequiredService(filterType);
                filter?.Apply(operation, filterContext);
            }

            return operation;
        }

        /// <summary>
        /// Generate the an operation of an AsyncApi Channel for the given class.
        /// </summary>
        private static Operation GenerateOperationFromClass(TypeInfo type, AsyncApiSchemaResolver schemaResolver, OperationType operationType, JsonSchemaGenerator jsonSchemaGenerator)
        {
            var operationAttribute = GetOperationAttribute(type, operationType);
            if (operationAttribute == null)
            {
                return null;
            }

            var messages = new Messages();
            var operation = new Operation
            {
                OperationId = operationAttribute.OperationId ?? type.Name,
                Summary = operationAttribute.Summary ?? type.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (type.GetXmlDocsRemarks() != "" ? type.GetXmlDocsRemarks() : null),
                Message = messages,
                Bindings = operationAttribute.BindingsRef != null ? new OperationBindingsReference(operationAttribute.BindingsRef) : null,
            };

            var methodsWithMessageAttribute = type.DeclaredMethods
                .Select(method => new
                {
                    MessageAttributes = method.GetCustomAttributes<MessageAttribute>(),
                    Method = method,
                })
                .Where(mm => mm.MessageAttributes.Any());

            foreach (MessageAttribute messageAttribute in methodsWithMessageAttribute.SelectMany(x => x.MessageAttributes))
            {
                var message = GenerateMessageFromAttribute(messageAttribute, schemaResolver, jsonSchemaGenerator);
                if (message != null)
                {
                    messages.OneOf.Add(message);
                }
            }

            if (messages.OneOf.Count == 1)
            {
                operation.Message = messages.OneOf.First();
            }

            return operation;
        }

        private static OperationAttribute GetOperationAttribute(MemberInfo typeOrMethod, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Publish:
                    var publishOperationAttribute = typeOrMethod.GetCustomAttribute<PublishOperationAttribute>();
                    return (OperationAttribute) publishOperationAttribute;

                case OperationType.Subscribe:
                    var subscribeOperationAttribute = typeOrMethod.GetCustomAttribute<SubscribeOperationAttribute>();
                    return (OperationAttribute) subscribeOperationAttribute;

                default:
                    return null;
            }
        }

        private static IMessage GenerateMessageFromAttributes(IEnumerable<MessageAttribute> messageAttributes, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
        {
            if (messageAttributes.Count() == 1)
            {
                return GenerateMessageFromAttribute(messageAttributes.First(), schemaResolver, jsonSchemaGenerator);
            }

            var messages = new Messages();
            foreach (MessageAttribute messageAttribute in messageAttributes)
            {
                var message = GenerateMessageFromAttribute(messageAttribute, schemaResolver, jsonSchemaGenerator);
                if (message != null)
                {
                    messages.OneOf.Add(message);
                }
            }

            if (messages.OneOf.Count == 1)
            {
                return messages.OneOf.First();
            }

            return messages;
        }

        private static IMessage GenerateMessageFromAttribute(MessageAttribute messageAttribute, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
        {
            if (messageAttribute?.PayloadType == null)
            {
                return null;
            }

            var message = new Message
            {
                MessageId = messageAttribute.MessageId,
                Payload = jsonSchemaGenerator.Generate(messageAttribute.PayloadType, schemaResolver),
                Title = messageAttribute.Title,
                Summary = messageAttribute.Summary,
                Description = messageAttribute.Description,
                Bindings = messageAttribute.BindingsRef != null ? new MessageBindingsReference(messageAttribute.BindingsRef) : null,
            };
            message.Name = messageAttribute.Name ?? message.Payload.ActualSchema.Id;

            return schemaResolver.GetMessageOrReference(message);
        }
        

        private static IMessage GenerateMessageFromType(Type payloadType, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
        {
            if (payloadType == null)
            {
                return null;
            }

            var message = new Message
            {
                Payload = jsonSchemaGenerator.Generate(payloadType, schemaResolver),
            };
            message.Name = message.Payload.Id;

            return schemaResolver.GetMessageOrReference(message);
        }

        private static IDictionary<string,IParameter> GetChannelParametersFromAttributes(MemberInfo memberInfo, AsyncApiSchemaResolver schemaResolver, JsonSchemaGenerator jsonSchemaGenerator)
        {
            IEnumerable<ChannelParameterAttribute> attributes = memberInfo.GetCustomAttributes<ChannelParameterAttribute>();
            var parameters = new Dictionary<string, IParameter>();
            if (attributes.Any())
            {
                foreach (ChannelParameterAttribute attribute in attributes)
                {
                    var parameter = schemaResolver.GetParameterOrReference(new Parameter
                    {
                        Description = attribute.Description,
                        Name = attribute.Name,
                        Schema = jsonSchemaGenerator.Generate(attribute.Type, schemaResolver),
                        Location = attribute.Location,
                    });
                    
                    parameters.Add(attribute.Name, parameter);
                }
            }

            return parameters;
        }
    }
}