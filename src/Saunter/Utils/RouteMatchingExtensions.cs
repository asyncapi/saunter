using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace Saunter.Utils
{
    public static class RouteMatchingExtensions
    {
        public static bool IsMatchingRoute(this PathString path, string pattern)
        {
            var template = TemplateParser.Parse(pattern);

            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);
            return matcher.TryMatch(path, values);
        }

        public static bool TryGetDocument(this HttpContext context, AsyncApiOptions options, out string document)
        {
            document = null;
#if NETSTANDARD2_0
            var template = TemplateParser.Parse(options.Middleware.Route);

            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);
            matcher.TryMatch(context.Request.Path, values);
#else
            var values = context.Request.RouteValues;
#endif
            if (!values.TryGetValue("document", out var value))
            {
                return false;
            }

            document = value.ToString();
            return true;
        }
    }
}
