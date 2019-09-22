using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.Attributes;
using Saunter.Attributes.Bindings.Amqp;
using Saunter.Microsoft.Extensions.DependencyInjection;

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
            services.AddSaunter(
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

        [Test]
        public void Test2()
        {
            var services = new ServiceCollection() as IServiceCollection;
            services.AddSaunter(
                options =>
                {
                    options.AsyncApiSchema = new AsyncApiSchema.v2_0_0.AsyncApiSchema();
                    options.AssemblyMarkerTypes.Add(this.GetType());
                });

            var sp = services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IAsyncApiSchemaProvider>();
            
            var schema = provider.GetSchema();
            
            var json = JsonConvert.SerializeObject(schema, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            Debug.WriteLine(json);
            
            Assert.That(schema.Channels, Is.Not.Null);
        }
    }

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
    
    [AsyncApi] // todo: this kind of sucks, and is easy to forget to do
    [AmqpTopic("exampleTopic")] // todo: while this lets you write less code, it's also confusing as this is a channelbinding, but not attached to the channel which is on each method  
    public class MessagePublisher
    {
        private readonly IAmqpClient amqp;

        public MessagePublisher(IAmqpClient amqp)
        {
            this.amqp = amqp;
        }

        [Channel("exampleRoutingKey", Description = "This is an example message channel")]
        [Publish(PayloadType = typeof(ExampleMessage), HeadersType = typeof(ExampleHeaders), ContentType = "application/json")]
        public void PublishExampleMessage()
        {
            var topic = this.GetType().GetCustomAttribute<AmqpTopicAttribute>().Topic;
            var routingKey = MethodBase.GetCurrentMethod().GetCustomAttribute<ChannelAttribute>().Name;
            
            var message = new ExampleMessage();
            var payload = JsonConvert.SerializeObject(message);
            
            amqp.Publish(topic, routingKey, payload);
        }
    }
}
