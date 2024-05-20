using System.Collections.Generic;
using System.Reflection;

using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Generation.Filters;

public class DocumentFilterTests
{
    [Fact]
    public void DocumentFilterIsAppliedToAsyncApiDocument()
    {
        // Arrange
        AsyncApiOptions options = new();
        DocumentGenerator documentGenerator = new();

        // Act
        options.AddDocumentFilter<ExampleDocumentFilter>();
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { GetType().GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.Channels.Count.ShouldBe(1);
        document.Channels.ShouldContainKey("foo");
    }

    [Fact]
    public void DocumentNameIsAppliedToAsyncApiDocument()
    {
        // Arrange
        const string documentName = "Test Document";
        AsyncApiOptions options = new();
        options.AsyncApi.DocumentName = documentName;
        DocumentGenerator documentGenerator = new();

        // Act
        options.AddDocumentFilter<ExampleDocumentFilter>();
        AsyncApiDocument document = documentGenerator.GenerateDocument(new[] { GetType().GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

        // Assert
        document.ShouldNotBeNull();
        document.DocumentName.ShouldBe(documentName);
    }

    private class ExampleDocumentFilter : IDocumentFilter
    {
        public void Apply(AsyncApiDocument document, DocumentFilterContext context)
        {
            ChannelItem channel = new()
            {
                Description = "an example channel for testing"
            };

            document.Channels.Add(new KeyValuePair<string, ChannelItem>("foo", channel));
        }
    }
}
