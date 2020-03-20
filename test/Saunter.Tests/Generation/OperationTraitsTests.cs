using Saunter.AsyncApiSchema.v2;
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
            
            var provider = TestProviderFactory.Provider(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Components =
                    {
                        OperationTraits =
                        {
                            ["exampleTrait"] = new OperationTrait { Description = "This is an example trait" }
                        }
                    }
                };
                
                o.PublishOperationFilters.Add(new TestOperationTraitsFilter());
            });

            var document = provider.GetDocument();
            
            document.Components.OperationTraits.ShouldContainKey("exampleTrait");
        }
        
        
        private class TestOperationTraitsFilter : IPublishOperationFilter
        {
            public void Apply(Operation publishOperation, PublishOperationFilterContext context)
            {
                publishOperation.Traits.Add(new OperationTraitReference("exampleTrait"));
            }
        }
    }
}