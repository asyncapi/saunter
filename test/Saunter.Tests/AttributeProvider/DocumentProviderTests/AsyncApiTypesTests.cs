using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.AttributeProvider.Attributes;
using Saunter.Options;
using Saunter.Tests.AttributeProvider.DocumentGenerationTests;
using Saunter.Tests.MarkerTypeTests;
using Shouldly;
using Xunit;

namespace Saunter.Tests.AttributeProvider.DocumentProviderTests
{
    public class AsyncApiTypesTests
    {
        [Fact]
        public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
        {
            var services = new ServiceCollection() as IServiceCollection;

            services.AddFakeLogging();
            services.AddAsyncApiSchemaGeneration(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Info = new()
                    {
                        Title = GetType().FullName,
                        Version = "1.0.0"
                    },
                };
                o.AssemblyMarkerTypes = new[] { typeof(AnotherSamplePublisher), typeof(SampleConsumer) };
            });

            using var serviceprovider = services.BuildServiceProvider();

            var documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
            var options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
            var document = documentProvider.GetDocument(null, options);

            document.ShouldNotBeNull();
        }

        [Fact]
        public void GetDocument_PreservesDefaultTypeSelection()
        {
            ArrangeAttributesTests.Arrange(
                out var options,
                out var documentProvider,
                typeof(AttributedPublisher),
                typeof(AttributeFreePublisher));

            var document = documentProvider.GetDocument(null, options);

            document.Channels.ShouldContainKey("attributed");
            document.Channels.ShouldContainKey("attribute-free");
        }

        [Fact]
        public void GetDocument_CustomTypeSelectorCanIncludeAndExcludeTypes()
        {
            ArrangeAttributesTests.Arrange(
                out var options,
                out var documentProvider,
                typeof(AttributedPublisher),
                typeof(AttributeFreePublisher));

            options.AsyncApiTypeSelector = (type, _) => type == typeof(AttributeFreePublisher);

            var document = documentProvider.GetDocument(null, options);

            document.Channels.ShouldContainKey("attribute-free");
            document.Channels.ShouldNotContainKey("attributed");
        }

        [Fact]
        public void GetDocument_CustomTypeSelectorReceivesDocumentName()
        {
            const string DocumentName = "orders";
            string selectedDocumentName = null;

            ArrangeAttributesTests.Arrange(
                out var options,
                out var documentProvider,
                typeof(AttributeFreePublisher));

            options.AsyncApiTypeSelector = (type, documentName) =>
            {
                selectedDocumentName = documentName;
                return type == typeof(AttributeFreePublisher) && documentName == DocumentName;
            };

            var document = documentProvider.GetDocument(DocumentName, options);

            selectedDocumentName.ShouldBe(DocumentName);
            document.Channels.ShouldContainKey("attribute-free");
        }

        [AsyncApi]
        [Channel("attributed")]
        [PublishOperation]
        private class AttributedPublisher
        {
            [Message(typeof(TestMessage))]
            public void Publish(TestMessage _) { }
        }

        [Channel("attribute-free")]
        [PublishOperation]
        private class AttributeFreePublisher
        {
            [Message(typeof(TestMessage))]
            public void Publish(TestMessage _) { }
        }

        private class TestMessage { }
    }
}
