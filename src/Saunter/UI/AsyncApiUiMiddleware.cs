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
using Saunter.Middleware;
using Saunter.Options;

namespace Saunter.UI
{
    internal class AsyncApiUiMiddleware
    {
        private readonly AsyncApiOptions _options;
        private readonly StaticFileMiddleware _staticFiles;
        private readonly Dictionary<string, StaticFileMiddleware> _namedStaticFiles;

        public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            _options = options.Value;
            var fileProvider = new EmbeddedFileProvider(GetType().Assembly, GetType().Namespace);
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = UiBaseRoute,
                FileProvider = fileProvider,
            };
            _staticFiles = new StaticFileMiddleware(next, env, Microsoft.Extensions.Options.Options.Create(staticFileOptions), loggerFactory);
            _namedStaticFiles = new Dictionary<string, StaticFileMiddleware>();

            foreach (var namedApi in _options.NamedApis)
            {
                var namedStaticFileOptions = new StaticFileOptions
                {
                    RequestPath = UiBaseRoute.Replace("{document}", namedApi.Key),
                    FileProvider = fileProvider,
                };
                _namedStaticFiles.Add(namedApi.Key, new StaticFileMiddleware(next, env, Microsoft.Extensions.Options.Options.Create(namedStaticFileOptions), loggerFactory));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsRequestingUiBase(context.Request))
            {
                context.Response.StatusCode = (int)HttpStatusCode.MovedPermanently;

                if (context.TryGetDocument(out var document))
                {
                    context.Response.Headers["Location"] = GetUiIndexFullRoute(context.Request).Replace("{document}", document);
                }
                else
                {
                    context.Response.Headers["Location"] = GetUiIndexFullRoute(context.Request);
                }
                return;
            }

            if (IsRequestingAsyncApiUi(context.Request))
            {
                if (context.TryGetDocument(out var document))
                {
                    await RespondWithAsyncApiHtml(context.Response, GetDocumentFullRoute(context.Request).Replace("{document}", document));
                }
                else
                {
                    await RespondWithAsyncApiHtml(context.Response, GetDocumentFullRoute(context.Request));
                }
                return;
            }

            if (!context.TryGetDocument(out var documentName))
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

        private async Task RespondWithAsyncApiHtml(HttpResponse response, string route)
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream($"{GetType().Namespace}.index.html"))
            using (var reader = new StreamReader(stream))
            {
                var indexHtml = new StringBuilder(await reader.ReadToEndAsync());

                // Replace dynamic content such as the AsyncAPI document url
                foreach (var replacement in new Dictionary<string, string>
                {
                    ["{{title}}"] = _options.Middleware.UiTitle,
                    ["{{asyncApiDocumentUrl}}"] = route,
                })
                {
                    indexHtml.Replace(replacement.Key, replacement.Value);
                }

                response.StatusCode = (int)HttpStatusCode.OK;
                response.ContentType = MediaTypeNames.Text.Html;
                await response.WriteAsync(indexHtml.ToString(), Encoding.UTF8);
            }
        }

        private bool IsRequestingUiBase(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) && request.Path.IsMatchingRoute(UiBaseRoute);
        }

        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) && request.Path.IsMatchingRoute(UiIndexRoute);
        }

        private string UiIndexRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') + "/index.html";

        private string GetUiIndexFullRoute(HttpRequest request)
        {
            if (request.PathBase != null)
            {
                return request.PathBase.Add(UiIndexRoute);
            }

            return UiIndexRoute;
        }

        private string UiBaseRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') ?? string.Empty;

        private string GetDocumentFullRoute(HttpRequest request)
        {
            if (request.PathBase != null)
            {
                return request.PathBase.Add(_options.Middleware.Route);
            }

            return _options.Middleware.Route;
        }
    }
}
