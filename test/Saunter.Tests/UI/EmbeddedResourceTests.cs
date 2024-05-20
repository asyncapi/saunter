using Saunter.UI;

using Shouldly;

using Xunit;

namespace Saunter.Tests.UI;

public class EmbeddedResourceTests
{
    [Fact]
    public void SaunterBinaryContainsEmbeddedResourcesFromProject()
    {
        string[] resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
        resources.ShouldContain("Saunter.UI.index.html");
    }

    [Fact]
    public void SaunterBinaryContainsEmbeddedResourcesFromNpm()
    {
        string err = "Execute `npm install` from the src/Saunter.UI directory to install the UI dependencies";

        string[] resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
        resources.ShouldContain("Saunter.UI.default.min.css", customMessage: err);
        resources.ShouldContain("Saunter.UI.index.js", customMessage: err);
    }
}
