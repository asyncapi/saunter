using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Saunter.AsyncApiSchema.v2;
using Saunter.AsyncApiSchema.v2.Traits;
using Saunter.Generation.Filters;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Generation;

public class OperationTraitsTests
{
    [Fact]
    public void Example_OperationTraits()
    {
        // TODO: this is not really a test, just an example of how you might use OperationTraits...

        IServiceCollection services = new ServiceCollection();
        services.AddAsyncApiSchemaGeneration(o =>
        {
            o.AsyncApi = new AsyncApiDocument
            {
                Info = new Info { Title = GetType().FullName, Version = "1.0.0" },
                Components = new()
                {
                    OperationTraits =
                    {
                        ["exampleTrait"] = new OperationTrait { OperationId = "exampleTrait", Description = "This is an example trait" }
                    }
                }
            };

            o.AddOperationFilter<TestOperationTraitsFilter>();
        });

        using ServiceProvider serviceprovider = services.BuildServiceProvider();
        IAsyncApiDocumentProvider documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
        AsyncApiOptions options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
        AsyncApiDocument document = documentProvider.GetDocument(options, options.AsyncApi);

        document.Components.OperationTraits.ShouldContainKey("exampleTrait");
    }


    private class TestOperationTraitsFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Traits.Add(new OperationTraitReference("exampleTrait"));
        }
    }
}