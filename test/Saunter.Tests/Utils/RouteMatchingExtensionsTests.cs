using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.Utils;
using Shouldly;
using Xunit;

namespace Saunter.Tests.Utils;
public class RouteMatchingExtensionsTests
{
    [Fact]
    public void TryGetDocument_Document_WhenRequestingJs()
    {
        var documentName = "Document";

        var context = new DefaultHttpContext();
        context.Request.Path = $"/asyncapi/{documentName}/ui/index.js";

        var services = new ServiceCollection()
            .ConfigureNamedAsyncApi(documentName, api =>
            {

            })
            .BuildServiceProvider();

        var options = services.GetRequiredService<IOptions<AsyncApiOptions>>().Value;

        var result = context.TryGetDocument(options, out var document);

        result.ShouldBeTrue();
        document.ShouldBe(documentName);
    }
}
