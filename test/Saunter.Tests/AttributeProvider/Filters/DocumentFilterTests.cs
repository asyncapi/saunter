using System.Collections.Generic;
using LEGO.AsyncAPI.Models;
using Saunter.Options.Filters;
using Saunter.Tests.AttributeProvider.DocumentGenerationTests;
using Shouldly;
using Xunit;

namespace Saunter.Tests.AttributeProvider.Filters
{
    public class DocumentFilterTests
    {
        [Fact]
        public void DocumentFilterIsAppliedToAsyncApiDocument()
        {
            // Arrange
            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, GetType());

            options.AddDocumentFilter<ExampleDocumentFilter>();

            // Act
            var document = documentProvider.GetDocument(null, options);

            // Assert
            document.ShouldNotBeNull();
            document.Channels.Count.ShouldBe(1);
            document.Channels.ShouldContainKey("foo");
        }

        [Fact]
        public void DocumentNameIsAppliedToAsyncApiDocument()
        {
            // Arrange
            const string DocumentName = "Test Document";

            ArrangeAttributesTests.Arrange(out var options, out var documentProvider, GetType());

            options.NamedApis[DocumentName] = new();
            options.AddDocumentFilter<ExampleDocumentFilter>();

            // Act
            var document = documentProvider.GetDocument(DocumentName, options);

            // Assert
            document.ShouldNotBeNull();
        }

        private class ExampleDocumentFilter : IDocumentFilter
        {
            public void Apply(AsyncApiDocument document, DocumentFilterContext context)
            {
                var channel = new AsyncApiChannel
                {
                    Description = "an example channel for testing"
                };

                document.Channels.Add(new KeyValuePair<string, AsyncApiChannel>("foo", channel));
            }
        }
    }
}
