using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;

using Shouldly;

using Xunit;

namespace AsyncApi.Net.Generator.Tests;

/// <remarks>
/// These are not real tests! Tests will be added once the API is semi-stable...
/// </remarks>
public class ServiceCollectionTests
{
    [Fact]
    public void TestAddAsyncApiSchemaGeneration()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddAsyncApiSchemaGeneration(options =>
            {
                options.AsyncApi = new AsyncApiDocument
                {
                    Id = "urn:com:example:example-events",
                    Info = new Info
                    {
                        Version = "2019.01.12345",
                        Title = "Example API",
                        Description = "An example API with events",
                        Contact = new Contact
                        {
                            Email = "michael@mwild.me",
                            Name = "Michael Wildman",
                            Url = "https://mwild.me/",
                        },
                        License = new License
                        {
                            Name = "MIT",
                        },
                        TermsOfService = "https://mwild.me/tos",
                    },
                    Tags = { "example", "event" },
                    Servers =
                    {
                        {
                            "development",
                            new Server
                            {
                                Url = "rabbitmq.dev.mwild.me",
                                Protocol = "amqp",
                                Security = new List<Dictionary<string, List<string>>> { new Dictionary<string, List<string>> { { "user-password", new List<string>() } }}
                            }
                        }
                    },
                    Components =
                    {
                        SecuritySchemes = new Dictionary<string, SecurityScheme>
                        {
                            { "user-password", new SecurityScheme
                                {
                                    Type = SecuritySchemeType.Http,
                                    In = "header",
                                    Scheme = "bearer",
                                    Name = "authorization",
                                    OpenIdConnectUrl = "https://helpme/pls",
                                    Flows = new(),
                                }
                            }
                        }
                    }
                };
            });

        ServiceProvider sp = services.BuildServiceProvider();

        IAsyncApiDocumentProvider provider = sp.GetRequiredService<IAsyncApiDocumentProvider>();

        AsyncApiDocument prototype = new()
        {
            Info = new()
            {
                Title = "tester",
                Version = "1.0",
            }
        };

        AsyncApiDocument document = provider.GetDocument(new AsyncApiOptions() { AsyncApi = prototype }, prototype);

        document.ShouldNotBeNull();
    }
}
