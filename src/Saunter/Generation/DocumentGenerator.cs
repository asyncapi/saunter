using Microsoft.Extensions.Options;
using Namotion.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Saunter.Generation
{
    public class DocumentGenerator : IDocumentGenerator
    {
        private readonly ISchemaGenerator _schemaGenerator;
        private readonly AsyncApiOptions _options;

        public DocumentGenerator(IOptions<AsyncApiOptions> options, ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public AsyncApiSchema.v2.AsyncApiDocument GenerateDocument(TypeInfo[] asyncApiTypes)
        {
            var schemaRepository = new SchemaRepository();

            var asyncApiSchema = _options.AsyncApi;
            
            asyncApiSchema.Channels = GenerateChannels(asyncApiTypes, schemaRepository);
            asyncApiSchema.Components.Schemas = schemaRepository.Schemas;

            var filterContext = new DocumentFilterContext(asyncApiTypes, schemaRepository);
            foreach (var filter in _options.DocumentFilters)
            {
                filter.Apply(asyncApiSchema, filterContext);
            }

            return asyncApiSchema;
        }


        
        
        /// <summary>
        /// Generate the Channels section of an AsyncApi schema. 
        /// </summary>
        private Channels GenerateChannels(TypeInfo[] asyncApiTypes, ISchemaRepository schemaRepository)
        {
            var channels = new Channels();
            
            channels.AddRange(GenerateChannelsFromMethods(asyncApiTypes, schemaRepository));
            channels.AddRange(GenerateChannelsFromClasses(asyncApiTypes, schemaRepository));

            return channels;
        }


        /// <summary>
        /// Generate the Channels section of the AsyncApi schema from the <see cref="ChannelAttribute"/> on methods.
        /// </summary>
        private Channels GenerateChannelsFromMethods(IEnumerable<TypeInfo> asyncApiTypes, ISchemaRepository schemaRepository)
        {
            var channels = new Channels();

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
                var channelItem = new ChannelItem
                {              
                    Description = mc.Channel!.Description,
                    Parameters = mc.Channel!.Parameters,
                    Publish = GenerateOperationFromMethod(mc.Method, schemaRepository, OperationType.Publish),
                    Subscribe = GenerateOperationFromMethod(mc.Method, schemaRepository, OperationType.Subscribe),
                }; 
                channels.Add(mc.Channel!.Name, channelItem);

                
                var context = new ChannelItemFilterContext(mc.Method, schemaRepository, mc.Channel);
                foreach (var filter in _options.ChannelItemFilters)
                {
                    filter.Apply(channelItem, context);
                }
            }

            return channels;
        }

        /// <summary>
        /// Generate the Channels section of the AsyncApi schema from the <see cref="ChannelAttribute"/> on classes.
        /// </summary>
        private Channels GenerateChannelsFromClasses(IEnumerable<TypeInfo> asyncApiTypes, ISchemaRepository schemaRepository)
        {
            var channels = new Channels();

            var classesWithChannelAttribute = asyncApiTypes
                .Select(type => new
                {
                    Channel = type.GetCustomAttribute<ChannelAttribute>(),
                    Type = type,
                })
                .Where(cc => cc.Channel != null);

            foreach (var cc in classesWithChannelAttribute)
            {

                var channelItem = new ChannelItem
                {
                    Description = cc.Channel!.Description,
                    Parameters = cc.Channel!.Parameters,
                    Publish = GenerateOperationFromClass(cc.Type, schemaRepository, OperationType.Publish),
                    Subscribe = GenerateOperationFromClass(cc.Type, schemaRepository, OperationType.Subscribe),                    
                };
                
                channels.Add(cc.Channel!.Name, channelItem);


                var context = new ChannelItemFilterContext(cc.Type, schemaRepository, cc.Channel);
                foreach (var filter in _options.ChannelItemFilters)
                {
                    filter.Apply(channelItem, context);
                }
            }

            return channels;
        }
        


        /// <summary>
        /// Generate the an operation of an AsyncApi Channel for the given method.
        /// </summary>
        private Operation? GenerateOperationFromMethod(MethodInfo method, ISchemaRepository schemaRepository, OperationType operationType)
        {
            var operationAttribute = GetOperationAttribute(method, operationType);
            if (operationAttribute == null)
            {
                return null;
            }

            var messageAttribute = method.GetCustomAttribute<MessageAttribute>();
            var message = messageAttribute != null
                ? GenerateMessageFromAttribute(messageAttribute, schemaRepository)
                : GenerateMessageFromType(operationAttribute.MessagePayloadType, schemaRepository);
            
            var operation = new Operation
            {
                OperationId = operationAttribute.OperationId ?? method.Name,
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (method.GetXmlDocsRemarks() != "" ? method.GetXmlDocsRemarks() : null),
                Message = message,
            };

            var filterContext = new OperationFilterContext(method, schemaRepository, operationAttribute);
            foreach (var filter in _options.OperationFilters)
            {
                filter.Apply(operation, filterContext);
            }

            return operation;
        }


        /// <summary>
        /// Generate the an operation of an AsyncApi Channel for the given class.
        /// </summary>
        private Operation? GenerateOperationFromClass(TypeInfo type, ISchemaRepository schemaRepository, OperationType operationType)
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
            };

            var methodsWithMessageAttribute = type.DeclaredMethods
                .Select(method => new
                {
                    Message = method.GetCustomAttribute<MessageAttribute>(),
                    Method = method,
                })
                .Where(mm => mm.Message != null);

            foreach (var mm in methodsWithMessageAttribute)
            {
                var message = GenerateMessageFromAttribute(mm.Message, schemaRepository);
                if (message != null)
                    messages.OneOf.Add(message);
            }

            return operation;
        }

        private static OperationAttribute? GetOperationAttribute(MemberInfo typeOrMethod, OperationType operationType)
        {
            switch (operationType)
            {
                case OperationType.Publish:
                    var publishOperationAttribute = typeOrMethod.GetCustomAttribute<PublishOperationAttribute>();
                    return (OperationAttribute?) publishOperationAttribute;
                
                case OperationType.Subscribe:
                    var subscribeOperationAttribute = typeOrMethod.GetCustomAttribute<SubscribeOperationAttribute>();
                    return (OperationAttribute?) subscribeOperationAttribute;
                
                default:
                    return null;
            }
        }

        private Message? GenerateMessageFromAttribute(MessageAttribute? messageAttribute, ISchemaRepository schemaRepository)
        {
            if (messageAttribute?.PayloadType == null)
            {
                return null;
            }
            
            var message = new Message
            {
                Payload = _schemaGenerator.GenerateSchema(messageAttribute.PayloadType, schemaRepository),
                Name = messageAttribute.Name ?? _options.SchemaIdSelector(messageAttribute.PayloadType),
                Title = messageAttribute.Title,
                Summary = messageAttribute.Summary,
                Description = messageAttribute.Description,
            };

            return message;
        }
        

        private Message? GenerateMessageFromType(Type? payloadType, ISchemaRepository schemaRepository)
        {
            if (payloadType == null)
            {
                return null;
            }
            
            var message = new Message
            {
                Payload = _schemaGenerator.GenerateSchema(payloadType, schemaRepository),
                Name = _options.SchemaIdSelector(payloadType),
            };

            return message;
        }
    }
}