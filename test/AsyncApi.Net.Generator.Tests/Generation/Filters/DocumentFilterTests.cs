using System.Reflection;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.Generation;
using AsyncApi.Net.Generator.Generation.Filters;

using Shouldly;

using Xunit;

namespace AsyncApi.Net.Generator.Tests.Generation.Filters;

public class DocumentFilterTests
{
    [Fact]
    public void DocumentFilterIsAppliedToAsyncApiDocument()
    {
        // Arrange
        AsyncApiOptions options = new()
        {
            AsyncApi = new()
            {
                Info = new()
                {
                    Version = "1.0.0",
                    Title = GetType().FullName,
                }
            }
        };
        DocumentGenerator documentGenerator = new();

        // Act
        options.AddDocumentFilter<ExampleDocumentFilter>();
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { GetType().GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);
        document.Channels.ShouldContainKey("foo");
    }


    private class ExampleDocumentFilter : IDocumentFilter
    {
        public void Apply(AsyncApiDocument document, DocumentFilterContext context)
        {
            Channel channel = new()
            {
                Description = "an example channel for testing"
            };

            document.Channels.Add("foo", channel);
        }
    }
}