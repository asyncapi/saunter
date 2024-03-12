using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Saunter;

internal static class HttpContextExtensions
{
    internal const string UriDocumentPlaceholder = "{document}";
    private const string UriDocumentPlaceholderEncoded = "%7Bdocument%7D";
    private const string UriDocumentFile = "/asyncapi.json";

    public static bool TryGetDocument(this HttpContext context, AsyncApiOptions options, out string documentName)
    {
        foreach (var documentNameSpecified in options.NamedApis.Values.Select(x => x.DocumentName))
        {
            var pathStart = options.Middleware.Route
                .Replace(UriDocumentPlaceholder, documentNameSpecified)
                .Replace(UriDocumentFile, string.Empty);

            if (!HttpMethods.IsGet(context.Request.Method)
                || !context.Request.Path.StartsWithSegments(pathStart))
            {
                continue;
            }

            documentName = documentNameSpecified;
            return true;
        }

        documentName = string.Empty;
        return false;
    }

    public static bool IsRequestingUiBase(this HttpContext context, AsyncApiOptions options)
    {
        var uiBaseRoute = options.Middleware.UiBaseRoute;
        return IsRequestingAsyncApiUrl(context, options, uiBaseRoute)
               || IsRequestingAsyncApiUrl(context, options, uiBaseRoute.TrimEnd('/'));
    }

    public static bool IsRequestingAsyncApiUi(this HttpContext context, AsyncApiOptions options)
    {
        var uiIndexRoute = options.Middleware.UiBaseRoute?.TrimEnd('/') + "/index.html";
        return context.IsRequestingAsyncApiUrl(options, uiIndexRoute);
    }

    public static bool IsRequestingAsyncApiDocument(this HttpContext context, AsyncApiOptions options)
    {
        var uiIndexRoute = options.Middleware.Route;
        return context.IsRequestingAsyncApiUrl(options, uiIndexRoute);
    }

    public static string GetAsyncApiUiIndexFullRoute(this HttpContext context, AsyncApiOptions options)
    {
        var uiIndexRoute = options.Middleware.UiBaseRoute?.TrimEnd('/') + "/index.html";
        return context.GetFullRoute(options, uiIndexRoute);
    }

    public static string GetAsyncApiDocumentFullRoute(this HttpContext context, AsyncApiOptions options)
    {
        var documentRoute = options.Middleware.Route;
        return context.GetFullRoute(options, documentRoute);
    }

    private static bool IsRequestingAsyncApiUrl(this HttpContext context, AsyncApiOptions options, string asyncApiBaseRoute)
    {
        if (context.TryGetDocument(options, out var documentName))
        {
            asyncApiBaseRoute = asyncApiBaseRoute.Replace(UriDocumentPlaceholder, documentName);
        }

        return HttpMethods.IsGet(context.Request.Method) && context.Request.Path.Equals(asyncApiBaseRoute);
    }

    private static string GetFullRoute(this HttpContext context, AsyncApiOptions options, string route)
    {
        if (context.TryGetDocument(options, out var documentName))
        {
            route = route
                .Replace(UriDocumentPlaceholder, documentName)
                .Replace(UriDocumentPlaceholderEncoded, documentName);
        }

        return context.Request.PathBase.Add(route);
    }
}