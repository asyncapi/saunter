using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.Generation;

namespace Saunter.Tests
{
    /// <remarks>
    /// These are not real tests! Tests will be added once the API is semi-stable...
    /// </remarks>
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

            Assert.That(schema, Is.Not.Null);
        }
    }
}
