using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.AsyncApiSchema.v2.Traits;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation
{
    public class OperationTraitsTests
    {
        [Fact]
        public void Example_OperationTraits()
        {
            // TODO: this is not really a test, just an example of how you might use OperationTraits...

            var services = new ServiceCollection() as IServiceCollection;
            services.AddAsyncApiSchemaGeneration(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Info = new Info(GetType().FullName, "1.0.0"),
                    Components =
                    {
                        OperationTraits =
                        {
                            ["exampleTrait"] = new OperationTrait { Description = "This is an example trait" }
                        }
                    }
                };

                o.AddOperationFilter<TestOperationTraitsFilter>();
            });

            using (var serviceprovider = services.BuildServiceProvider())
            {
                var documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
                var options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
                var document = documentProvider.GetDocument(options, options.AsyncApi);

                document.Components.OperationTraits.ShouldContainKey("exampleTrait");
            }
        }

        private class TestOperationTraitsFilter : IOperationFilter
        {
            public void Apply(Operation publishOperation, OperationFilterContext context)
            {
                publishOperation.Traits.Add(new OperationTraitReference("exampleTrait"));
            }
        }
    }
}
