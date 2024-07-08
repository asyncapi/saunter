using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Saunter.Middleware
{
    internal static class RouteMatchingExtensions
    {
        public static bool IsMatchingRoute(this PathString path, string pattern)
        {
            var template = TemplateParser.Parse(pattern);

            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);
            return matcher.TryMatch(path, values);
        }

        public static bool TryGetDocument(this HttpContext context, out string document)
        {
            var values = context.Request.RouteValues;
            if (!values.TryGetValue("document", out var value) || value == null)
            {
                document = null;
                return false;
            }

            document = value.ToString();
            return true;
        }
    }
}
