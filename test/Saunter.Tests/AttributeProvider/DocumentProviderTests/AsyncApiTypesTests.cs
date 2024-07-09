using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.Options;
using Saunter.Tests.MarkerTypeTests;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Generation.DocumentProviderTests
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
    }
}
