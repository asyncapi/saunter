#if !NETSTANDARD2_0

using System;
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

namespace Saunter.UI
{
    public class AsyncApiUiMiddleware
    {
        private readonly AsyncApiOptions _options;
        private readonly StaticFileMiddleware _staticFiles;


        public AsyncApiUiMiddleware(RequestDelegate next, IOptionsSnapshot<AsyncApiOptions> options, IWebHostEnvironment env, ILoggerFactory loggerFactory, string apiName = null)
        {
            if (apiName == null)
            {
                _options = options.Value;
            }
            else
            {
                _options = options.Get(apiName);
            }

            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = UiBaseRoute,
                FileProvider = new EmbeddedFileProvider(GetType().Assembly, GetType().Namespace),
            };
            _staticFiles = new StaticFileMiddleware(next, env, Options.Create(staticFileOptions), loggerFactory);
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsRequestingUiBase(context.Request))
            {
                context.Response.StatusCode = (int) HttpStatusCode.MovedPermanently;
                context.Response.Headers["Location"] = UiIndexRoute;
                return;
            }

            if (IsRequestingAsyncApiUi(context.Request))
            {
                await RespondWithAsyncApiHtml(context.Response);
                return;
            }

            await _staticFiles.Invoke(context);
        }

        private async Task RespondWithAsyncApiHtml(HttpResponse response)
        {
            await using var stream = GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.index.html");
            using var reader = new StreamReader(stream);
            var indexHtml = new StringBuilder(await reader.ReadToEndAsync());

            // Replace dynamic content such as the AsyncAPI document url
            foreach (var replacement in IndexHtmlReplacements)
            {
                indexHtml.Replace(replacement.Key, replacement.Value);
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = MediaTypeNames.Text.Html;
            await response.WriteAsync(indexHtml.ToString(), Encoding.UTF8);
        }

        private bool IsRequestingUiBase(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path.Value?.TrimEnd('/'), UiBaseRoute, StringComparison.OrdinalIgnoreCase);
        }
        
        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, UiIndexRoute, StringComparison.OrdinalIgnoreCase);
        }

        private Dictionary<string, string> IndexHtmlReplacements
            => new Dictionary<string, string>
            {
                ["{{title}}"] = _options.Middleware.UiTitle,
                ["{{asyncApiDocumentUrl}}"] = _options.Middleware.Route,
            };

        private string UiIndexRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') + "/index.html";

        private string UiBaseRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') ?? string.Empty;
    }
}
#endif