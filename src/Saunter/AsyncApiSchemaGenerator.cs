using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;
using Saunter.Attributes;
using Saunter.Attributes.Bindings.Amqp;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiSchemaGenerator : IAsyncApiSchemaProvider
    {
        private readonly AsyncApiGeneratorOptions options;

        public AsyncApiSchemaGenerator(IOptions<AsyncApiGeneratorOptions> options)
        {
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public AsyncApiSchema.v2_0_0.AsyncApiSchema GetSchema()
        {
            var asyncApiSchema = options.AsyncApiSchema;
            
            // todo: generate the channels section of the asyncapi schema
            // todo: build a SchemaRegistry like in Swashbuckle
            var channels = GetChannels();
            asyncApiSchema.Channels.AddRange(channels);
            
            
            // todo: validate asyncapi schema

            return asyncApiSchema;
        }

        /// <summary>
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies containing <see cref="AsyncApiGeneratorOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private IEnumerable<TypeInfo> GetAsyncApiTypes()
        {
            var assembliesToScan = options.AssemblyMarkerTypes.Select(t => t.Assembly);

            var asyncApiTypes = assembliesToScan
                .SelectMany(a => a.DefinedTypes.Where(t => t.HasCustomAttribute<AsyncApiAttribute>()));

            return asyncApiTypes;
        }
        
        
        
        private Channels GetChannels()
        {
            var channels = new Channels();

            var asyncapiTypes = GetAsyncApiTypes();

            foreach (var type in asyncapiTypes)
            {
                var methods = type.DeclaredMethods;

                foreach (var method in methods)
                {
                    var channelAttribute = method.GetCustomAttribute<ChannelAttribute>();
                    var publishAttribute = method.GetCustomAttribute<PublishOperationAttribute>();

                    if (channelAttribute != null)
                    {
                        var channel = new ChannelItem
                        {
                            Description = channelAttribute.Description,
                        };
                        channels.Add(channelAttribute.Name, channel);

                        if (publishAttribute != null)
                        {
                            channel.Publish = new Operation
                            {
                                Message = new Message
                                {
                                    Payload = NJsonSchema.JsonSchema.FromType(publishAttribute.PayloadType),
//                                    Headers = publishAttribute.HeadersType == null ? null : NJsonSchema.JsonSchema.FromType(publishAttribute.HeadersType),
//                                    ContentType = publishAttribute.ContentType,
                                }
                            };
                            
                            var amqpTopicAttribute = type.GetCustomAttribute<AmqpChannelBindingAttribute>();

                            if (amqpTopicAttribute != null)
                            {
                                channel.Bindings.Amqp = new AmqpChannelBinding
                                {
                                    Is = AmqpChannelBindingIs.RoutingKey,
                                    Exchange = new AmqpChannelBindingExchange
                                    {
                                        Name = amqpTopicAttribute.Name,
                                        Type = amqpTopicAttribute.ExchangeType,
                                    },
                                };
                            }
                        }
                    }
                }
            }
            
            return channels;
        }
        
    }
}