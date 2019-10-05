using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;
using Saunter.Attributes;
//using Saunter.Attributes.Bindings.Amqp;
using Saunter.Generation;

namespace Saunter.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var services = new ServiceCollection() as IServiceCollection;
            services.AddAsyncApiSchemaGeneration(
                options =>
                {
                    options.AsyncApiSchema = new AsyncApiSchema.v2_0_0.AsyncApiSchema
                    {
                        Id = new Identifier("urn:com:example:example-events"),
                        Info = new Info("Example API", "An example API with events")
                        {
                            Version = "2019.01.12345",
                            Contact = new Contact
                            {
                                Email = "michael@mwild.me",
                                Name = "Michael Wildman",
                                Url = "https://mwild.me/",
                            },
                            License = new License("MIT"),
                            TermsOfService = "https://mwild.me/tos",
                        },
                        Tags = { "example", "event" },
                        Servers =
                        {
                            { 
                                "development", 
                                new Server("rabbitmq.dev.mwild.me", "amqp")
                                {
                                    Security = new List<SecurityRequirement> { new SecurityRequirement { { "user-password", new List<string>() } }}
                                }
                            }
                        },
                        Components =
                        {
                            SecuritySchemes = new Dictionary<ComponentFieldName, SecurityScheme>
                            {
                                { "user-password", new SecurityScheme(SecuritySchemeType.Http) }
                            }
                        }
                    };
                });

            var sp = services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IAsyncApiSchemaProvider>();

            var schema = provider.GetSchema();
            var json = JsonConvert.SerializeObject(schema, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Debug.WriteLine(json);

            Assert.That(schema, Is.Not.Null);

        }

//        [Test]
//        public void Test2()
//        {
//            var services = new ServiceCollection() as IServiceCollection;
//            services.AddAsyncApiSchemaGeneration(
//                options =>
//                {
//                    options.AsyncApiSchema = new AsyncApiSchema.v2_0_0.AsyncApiSchema();
//                    options.AssemblyMarkerTypes.Add(this.GetType());
//
//                    AmqpDefaults.ChannelBinding.Is = AmqpChannelBindingIs.RoutingKey;
//                    AmqpDefaults.ChannelBinding.Name = "accounts";
//                    AmqpDefaults.ChannelBinding.ExchangeType = "topic";
//                });
//
//            var sp = services.BuildServiceProvider();
//
//            var provider = sp.GetRequiredService<IAsyncApiSchemaProvider>();
//            
//            var schema = provider.GetSchema();
//            
//            var json = JsonConvert.SerializeObject(schema, Formatting.Indented, new JsonSerializerSettings
//            {
//                NullValueHandling = NullValueHandling.Ignore
//            });
//            Debug.WriteLine(json);
//            
//            Assert.That(schema.Channels, Is.Not.Null);
//        }
//    }

    public class ExampleHeaders
    {
        public string MessageId { get; set; }
    }
    
    public class ExampleMessage
    {
        public string SomeProperty { get; set; }

        public int Abc { get; set; }
    }

    public interface IAmqpClient
    {
        void Publish(string topic, string routingKey, object payload);
    }
    
//    [AsyncApi]
//    public class MessagePublisher
//    {
//        private readonly IAmqpClient amqp;
//
//        public MessagePublisher(IAmqpClient amqp)
//        {
//            this.amqp = amqp;
//        }
//
//    
//        /// <summary>
//        /// Should be able to pull this description for the channel
//        /// </summary>
//        [Channel("example.routing.key")]
//        [AmqpChannelBinding]
////        [PublishOperation]
//        public void PublishExampleMessage2([Payload] ExampleMessage payload)
//        {
//            var exchangeName = MethodBase.GetCurrentMethod().GetCustomAttribute<AmqpChannelBindingAttribute>().Name;
//            var routingKey = MethodBase.GetCurrentMethod().GetCustomAttribute<ChannelAttribute>().Name;
//            
//            amqp.Publish(exchangeName, routingKey, payload);
//        }
    }
}
