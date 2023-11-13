using AsyncApi.Net.Generator.UI;

using Shouldly;

using Xunit;

namespace AsyncApi.Net.Generator.Tests.UI;

public class EmbeddedResourceTests
{
    [Fact]
    public void BinaryContainsEmbeddedResourcesFromProject()
    {
        string[] resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
        resources.ShouldContain("AsyncApi.Net.Generator.UI.index.html");
    }

    [Fact]
    public void BinaryContainsEmbeddedResourcesFromNpm()
    {
        string err = "Execute `npm install` from the src/AsyncApi.Net.Generator.UI directory to install the UI dependencies";

        string[] resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
        resources.ShouldContain("AsyncApi.Net.Generator.UI.default.min.css", customMessage: err);
        resources.ShouldContain("AsyncApi.Net.Generator.UI.index.js", customMessage: err);
    }
}