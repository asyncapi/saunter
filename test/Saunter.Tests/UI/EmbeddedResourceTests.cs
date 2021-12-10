using Saunter.UI;
using Shouldly;
using Xunit;

namespace Saunter.Tests.UI
{
    public class EmbeddedResourceTests
    {
        [Fact]
        public void SaunterBinaryContainsEmbeddedResourcesFromProject()
        {
            var resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
            resources.ShouldContain("Saunter.UI.index.html");
        }

        [Fact]
        public void SaunterBinaryContainsEmbeddedResourcesFromNpm()
        {
            var err = "Execute `npm install` from the src/Saunter.UI directory to install the UI dependencies";

            var resources = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceNames();
            resources.ShouldContain("Saunter.UI.default.min.css", customMessage: err);
            resources.ShouldContain("Saunter.UI.index.js", customMessage: err);
        }
    }
}