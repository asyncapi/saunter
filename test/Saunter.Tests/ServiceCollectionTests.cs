using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Saunter.Options;
using Shouldly;
using Xunit;

namespace Saunter.Tests
{
    /// <remarks>
    /// These are not real tests! Tests will be added once the API is semi-stable...
    /// </remarks>
    public class ServiceCollectionTests
    {
        [Fact]
        public void TestAddAsyncApiSchemaGeneration()
        {
            var services = new ServiceCollection() as IServiceCollection;

            services.AddFakeLogging();
            services.AddAsyncApiSchemaGeneration(options =>
                {
                    options.AsyncApi = new AsyncApiDocument
                    {
                        Id = "urn:com:example:example-events",
                        Info = new()
                        {
                            Title = "Example API",
                            Version = "2019.01.12345",
                            Description = "An example API with events",
                            Contact = new AsyncApiContact()
                            {
                                Email = "michael@mwild.me",
                                Name = "Michael Wildman",
                                Url = new("https://mwild.me/"),
                            },
                            License = new AsyncApiLicense()
                            {
                                Name = "MIT",
                            },
                            TermsOfService = new("https://mwild.me/tos"),
                        },
                        Tags =
                        {
                            new() { Name = "example" },
                            new() { Name = "event" }
                        },
                        Servers =
                        {
                            {
                                "development",
                                new AsyncApiServer
                                {
                                    Protocol = "amqp",
                                    Url = "rabbitmq.dev.mwild.me",
                                    Security = new List<AsyncApiSecurityRequirement>
                                    {
                                        new()
                                        {
                                            { new AsyncApiSecurityScheme() { Type= SecuritySchemeType.UserPassword }, new List<string>() }
                                        }
                                    }
                                }
                            }
                        },
                        Components =
                        {
                            SecuritySchemes = new Dictionary<string, AsyncApiSecurityScheme>
                            {
                                { "user-password", new AsyncApiSecurityScheme(){ Type = SecuritySchemeType.Http } }
                            }
                        }
                    };
                });

            var sp = services.BuildServiceProvider();

            var provider = sp.GetRequiredService<IAsyncApiDocumentProvider>();

            var document = provider.GetDocument(null, new AsyncApiOptions());

            document.ShouldNotBeNull();
        }
    }
}
