using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
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
            services.AddAsyncApiSchemaGeneration(o =>
            {
                o.AsyncApi = new AsyncApiDocument
                {
                    Info = new Info(GetType().FullName, "1.0.0")
                };
                o.AssemblyMarkerTypes = new[] { typeof(AnotherSamplePublisher), typeof(SampleConsumer) };
            });

            using (var serviceprovider = services.BuildServiceProvider())
            {
                var documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
                var options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
                var document = documentProvider.GetDocument(options, options.AsyncApi);

                document.ShouldNotBeNull();
            }
        }


    }
}
