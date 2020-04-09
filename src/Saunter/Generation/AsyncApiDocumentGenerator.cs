using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Namotion.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;
using Saunter.Utils;

namespace Saunter.Generation
{
    public class AsyncApiDocumentGenerator : IAsyncApiDocumentProvider
    {
        private readonly ISchemaGenerator _schemaGenerator;
        private readonly AsyncApiDocumentGeneratorOptions _options;

        public AsyncApiDocumentGenerator(IOptions<AsyncApiDocumentGeneratorOptions> options, ISchemaGenerator schemaGenerator)
        {
            _schemaGenerator = schemaGenerator;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public AsyncApiSchema.v2.AsyncApiDocument GetDocument()
        {
            var schemaRepository = new SchemaRepository();

            var asyncApiTypes = GetAsyncApiTypes();
            
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
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies containing <see cref="AsyncApiDocumentGeneratorOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private TypeInfo[] GetAsyncApiTypes()
        {
            var assembliesToScan = _options.AssemblyMarkerTypes.Select(t => t.Assembly);

            var asyncApiTypes = assembliesToScan
                .SelectMany(a => a.DefinedTypes.Where(t => t.HasCustomAttribute<AsyncApiAttribute>()))
                .ToArray();

            return asyncApiTypes;
        }
        
        
        /// <summary>
        /// Generate the Channels section of an AsyncApi schema. 
        /// </summary>
        private Channels GenerateChannels(IEnumerable<TypeInfo> asyncApiTypes, ISchemaRepository schemaRepository)
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
                    Description = mc.Channel.Description,
                    Parameters = mc.Channel.Parameters,
                    Publish = GenerateOperation(mc.Method, schemaRepository, OperationType.Publish),
                    Subscribe = GenerateOperation(mc.Method, schemaRepository, OperationType.Subscribe),
                }; 
                channels.Add(mc.Channel.Name, channelItem);
                
                var context = new ChannelItemFilterContext(mc.Method, schemaRepository, mc.Channel);
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
        private Operation GenerateOperation(MethodInfo method, ISchemaRepository schemaRepository, OperationType operationType)
        {
            OperationAttribute operationAttribute;
            switch (operationType)
            {
                case OperationType.Publish:
                    var publishOperationAttribute = method.GetCustomAttribute<PublishOperationAttribute>();
                    if (publishOperationAttribute == null)
                    {
                        return null;
                    }
                    operationAttribute = (OperationAttribute)publishOperationAttribute;
                    break;
                case OperationType.Subscribe:
                    var subscribeOperationAttribute = method.GetCustomAttribute<SubscribeOperationAttribute>();
                    if (subscribeOperationAttribute == null)
                    {
                        return null;
                    }
                    operationAttribute = (OperationAttribute)subscribeOperationAttribute;
                    break;
                default:
                    return null;
            }

            var operation = new Operation
            {
                OperationId = operationAttribute.OperationId ?? method.Name,
                Summary = operationAttribute.Summary ?? method.GetXmlDocsSummary(),
                Description = operationAttribute.Description ?? (method.GetXmlDocsRemarks() != "" ? method.GetXmlDocsRemarks() : null),
                Message = GenerateMessage(method, operationAttribute, schemaRepository),
            };

            var filterContext = new OperationFiterContext(method, schemaRepository, operationAttribute);
            foreach (var filter in _options.OperationFilters)
            {
                filter.Apply(operation, filterContext);
            }

            return operation;
        }

        private Message GenerateMessage(MethodInfo method, OperationAttribute operationAttribute, ISchemaRepository schemaRepository)
        {
            var message = new Message
            {
                Payload = _schemaGenerator.GenerateSchema(operationAttribute.MessagePayloadType, schemaRepository),
                // todo: all the other properties... message has a lot!
            };

            return message;
        }
    }
}