using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Saunter.AsyncApiSchema.v2;
using Saunter.Tests.MarkerTypeTests;

using Shouldly;

using Xunit;

namespace Saunter.Tests.Generation.DocumentProviderTests;

public class AsyncApiTypesTests
{
    [Fact]
    public void GetDocument_GeneratesDocumentWithMultipleMessagesPerChannel()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddAsyncApiSchemaGeneration(o =>
        {
            o.AsyncApi = new AsyncApiDocument
            {
                Info = new Info
                {
                    Title = GetType().FullName,
                    Version = "1.0.0",
                }
            };
            o.AssemblyMarkerTypes = new[] { typeof(AnotherSamplePublisher), typeof(SampleConsumer) };
        });

        using (ServiceProvider serviceprovider = services.BuildServiceProvider())
        {
            IAsyncApiDocumentProvider documentProvider = serviceprovider.GetRequiredService<IAsyncApiDocumentProvider>();
            AsyncApiOptions options = serviceprovider.GetRequiredService<IOptions<AsyncApiOptions>>().Value;
            AsyncApiDocument document = documentProvider.GetDocument(options, options.AsyncApi);

            document.ShouldNotBeNull();
        }
    }


}
