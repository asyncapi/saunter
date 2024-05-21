using System.Collections.Generic;
using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.Filters
{
    public class DocumentFilterTests
    {
        [Fact]
        public void DocumentFilterIsAppliedToAsyncApiDocument()
        {
            // Arrange
            var options = new AsyncApiOptions();
            var documentGenerator = new DocumentGenerator();

            // Act
            options.AddDocumentFilter<ExampleDocumentFilter>();
            var document = documentGenerator.GenerateDocument(new[] { GetType().GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

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
            var options = new AsyncApiOptions();
            options.AsyncApi.DocumentName = documentName;
            var documentGenerator = new DocumentGenerator();

            // Act
            options.AddDocumentFilter<ExampleDocumentFilter>();
            var document = documentGenerator.GenerateDocument(new[] { GetType().GetTypeInfo() }, options, options.AsyncApi, ActivatorServiceProvider.Instance);

            // Assert
            document.ShouldNotBeNull();
            document.DocumentName.ShouldBe(documentName);
        }

        private class ExampleDocumentFilter : IDocumentFilter
        {
            public void Apply(AsyncApiDocument document, DocumentFilterContext context)
            {
                var channel = new ChannelItem
                {
                    Description = "an example channel for testing"
                };

                document.Channels.Add(new KeyValuePair<string, ChannelItem>("foo", channel));
            }
        }
    }
}
