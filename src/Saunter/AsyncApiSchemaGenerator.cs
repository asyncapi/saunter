using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;
using Saunter.Attributes;
using Saunter.Attributes.Bindings.Amqp;

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
            var assembliesToScan = options.AssemblyMarkerTypes.Select(t => t.Assembly);

            foreach (var assembly in assembliesToScan)
            {
                var allTypes = assembly.DefinedTypes;

                var asyncapiTypes = allTypes.Where(t => t.GetCustomAttribute<AsyncApiAttribute>() != null);

                foreach (var type in asyncapiTypes)
                {
                    var amqpTopicAttribute = type.GetCustomAttribute<AmqpTopicAttribute>();
                    
                    var methods = type.DeclaredMethods;

                    foreach (var method in methods)
                    {
                        var channelAttribute = method.GetCustomAttribute<ChannelAttribute>();
                        var publishAttribute = method.GetCustomAttribute<PublishAttribute>();

                        if (channelAttribute != null)
                        {
                            var channel = new ChannelItem
                            {
                                Description = channelAttribute.Description,
                            };
                            asyncApiSchema.Channels.Add(channelAttribute.Name, channel);

                            if (publishAttribute != null)
                            {
                                channel.Publish = new Operation
                                {
                                    Message = new Message
                                    {
                                        Payload = NJsonSchema.JsonSchema.FromType(publishAttribute.PayloadType),
                                        Headers = publishAttribute.HeadersType == null ? null : NJsonSchema.JsonSchema.FromType(publishAttribute.HeadersType),
                                        ContentType = publishAttribute.ContentType,
                                    }
                                };

                                if (amqpTopicAttribute != null)
                                {
                                    channel.Bindings.Amqp = new AmqpChannelBinding
                                    {
                                        Is = AmqpChannelIs.RoutingKey,
                                        Exchange = new AmqpChannelExchange
                                        {
                                            Name = amqpTopicAttribute.Topic,
                                            Type = AmqpExchangeType.Topic,
                                        },
                                    };
                                }
                            }
                        }
                    }
                }

            }
            
            
            // todo: validate asyncapi schema

            return asyncApiSchema;
        }
    }
}