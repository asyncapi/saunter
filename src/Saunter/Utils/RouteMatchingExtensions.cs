using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Template;

namespace Saunter.Utils
{
    public static class RouteMatchingExtensions
    {
        public static bool IsMatchingRoute(this HttpRequest request, string pattern)
        {
            var template = TemplateParser.Parse(pattern);
            
            var matcher = new TemplateMatcher(template, request.RouteValues);
            return matcher.TryMatch(request.Path, request.RouteValues);
        }

        public static bool TryGetDocument(this HttpContext context, AsyncApiOptions options, out string document)
        {
            var values = context.Request.RouteValues;

            if (values.Count == 0)
            {
                var template = TemplateParser.Parse(options.Middleware.UiBaseRoute + "{*wildcard}");
                var matcher = new TemplateMatcher(template, values);
                matcher.TryMatch(context.Request.Path, values);
            }

            if (values.TryGetValue("document", out var value) && value != null)
            {
                document = value.ToString();
                return true;
            }

            document = null;
            return false;
        }
    }
}
