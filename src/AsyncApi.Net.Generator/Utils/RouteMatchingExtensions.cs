using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace AsyncApi.Net.Generator.Utils;

public static class RouteMatchingExtensions
{
    public static bool IsMatchingRoute(this PathString path, string pattern)
    {
        RouteTemplate template = TemplateParser.Parse(pattern);

        RouteValueDictionary values = [];
        TemplateMatcher matcher = new(template, values);
        return matcher.TryMatch(path, values);
    }

    public static bool TryGetDocument(this HttpContext context, out string? document)
    {
        RouteValueDictionary values = context.Request.RouteValues;
        if (!values.TryGetValue("document", out object? value) || value == null)
        {
            document = null;
            return false;
        }

        document = value.ToString();
        return true;
    }
}
