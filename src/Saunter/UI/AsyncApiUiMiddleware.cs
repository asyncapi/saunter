using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Saunter.UI;

internal sealed class AsyncApiUiMiddleware
{
    private readonly AsyncApiOptions _options;
    private readonly StaticFileMiddleware _staticFiles;
    private readonly Dictionary<string, StaticFileMiddleware> _namedStaticFiles;

    public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        _options = options.Value;
        var fileProvider = new EmbeddedFileProvider(typeof(AsyncApiUiMiddleware).Assembly, "Saunter.UI");
        var staticFileOptions = new StaticFileOptions { RequestPath = GetUiBaseRoute(), FileProvider = fileProvider, };
        _staticFiles = new StaticFileMiddleware(next, env, Options.Create(staticFileOptions), loggerFactory);
        _namedStaticFiles = new Dictionary<string, StaticFileMiddleware>();

        foreach (var namedApi in _options.NamedApis.Keys)
        {
            var namedStaticFileOptions = new StaticFileOptions { RequestPath = GetUiBaseRouteNamed(namedApi), FileProvider = fileProvider, };
            _namedStaticFiles.Add(namedApi, new StaticFileMiddleware(next, env, Options.Create(namedStaticFileOptions), loggerFactory));
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.IsRequestingUiBase(_options))
        {
            context.Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
            context.Response.Headers["Location"] = context.GetAsyncApiUiIndexFullRoute(_options);
            return;
        }

        if (context.IsRequestingAsyncApiUi(_options))
        {
            await RespondWithAsyncApiHtmlAsync(context.Response, context.GetAsyncApiDocumentFullRoute(_options));
            return;
        }

        if (!context.TryGetDocument(_options, out var documentName))
        {
            await _staticFiles.Invoke(context);
        }
        else
        {
            if (_namedStaticFiles.TryGetValue(documentName, out var files))
            {
                await files.Invoke(context);
            }
            else
            {
                await _staticFiles.Invoke(context);
            }
        }
    }

    private async Task RespondWithAsyncApiHtmlAsync(HttpResponse response, string route)
    {
        await using var stream = typeof(AsyncApiUiMiddleware).Assembly.GetManifestResourceStream("Saunter.UI.index.html");
        if (stream == null)
        {
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return;
        }

        using var reader = new StreamReader(stream);
        var indexHtml = new StringBuilder(await reader.ReadToEndAsync());

        // Replace dynamic content such as the AsyncAPI document url
        foreach (var replacement in new Dictionary<string, string> { ["{{title}}"] = _options.Middleware.UiTitle, ["{{asyncApiDocumentUrl}}"] = route, })
        {
            indexHtml.Replace(replacement.Key, replacement.Value);
        }

        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = MediaTypeNames.Text.Html;
        await response.WriteAsync(indexHtml.ToString(), Encoding.UTF8);
    }

    private string GetUiBaseRoute() => _options.Middleware.UiBaseRoute?.TrimEnd('/') ?? string.Empty;

    private string GetUiBaseRouteNamed(string documentName) => GetUiBaseRoute()
        .Replace(HttpContextExtensions.UriDocumentPlaceholder, documentName)
        .TrimEnd('/');
}