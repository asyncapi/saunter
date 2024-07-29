using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.Options;
using Saunter.Options.Filters;
using Shouldly;
using Xunit;

namespace Saunter.Tests.AttributeProvider
{
    public class OperationTraitsTests
    {
        [Fact]
        public void Example_OperationTraits()
        {
            // TODO: this is not really a test, just an example of how you might use OperationTraits...

            var services = new ServiceCollection() as IServiceCollection;

            services.AddFakeLogging();
            services.AddAsyncApiSchemaGeneration(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Info = new AsyncApiInfo
                    {
                        Title = GetType().FullName,
                        Version = "1.0.0"
                    },
                    Components = new()
                    {
                        OperationTraits =
                        {
                            ["exampleTrait"] = new AsyncApiOperationTrait { Description = "This is an example trait" }
                        }
                    }
                };

                o.AddOperationFilter<TestOperationTraitsFilter>();
            });

            using var serviceprovider = services.BuildServiceProvider();

            var documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
            var options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
            var document = documentProvider.GetDocument(null, options);

            document.Components.OperationTraits.ShouldContainKey("exampleTrait");
        }


        private class TestOperationTraitsFilter : IOperationFilter
        {
            public void Apply(AsyncApiOperation operation, OperationFilterContext context)
            {
                operation.Traits.Add(new AsyncApiOperationTrait()
                {
                    Reference = new()
                    {
                        Id = "exampleTrait",
                        Type = ReferenceType.OperationTrait,
                    },
                });
            }
        }
    }
}
