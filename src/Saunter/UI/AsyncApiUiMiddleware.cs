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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
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
        private readonly Dictionary<string, StaticFileMiddleware> _namedStaticFiles;

#if NETSTANDARD2_0
        public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IHostingEnvironment env, ILoggerFactory loggerFactory)
#else
        public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IWebHostEnvironment env, ILoggerFactory loggerFactory)
#endif
        {
            _options = options.Value;
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = UiBaseRoute,
                FileProvider = new EmbeddedFileProvider(GetType().Assembly, GetType().Namespace),
            };
            _staticFiles = new StaticFileMiddleware(next, env, Options.Create(staticFileOptions), loggerFactory);
            _namedStaticFiles = new Dictionary<string, StaticFileMiddleware>();

            foreach (var namedApi in _options.NamedApis)
            {
                var namedStaticFileOptions = new StaticFileOptions
                {
                    RequestPath = UiBaseRoute.Replace("{document}", namedApi.Key),
                    FileProvider = new EmbeddedFileProvider(GetType().Assembly, GetType().Namespace),
                };
                _namedStaticFiles.Add(namedApi.Key, new StaticFileMiddleware(next, env, Options.Create(namedStaticFileOptions), loggerFactory));
            }
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsRequestingUiBase(context.Request))
            {
                context.Response.StatusCode = (int) HttpStatusCode.MovedPermanently;
                context.Response.Headers["Location"] = UiIndexRoute;
                return;
            }

            {
                if (IsRequestingNamedUiBase(context.Request, out var document))
                {
                    context.Response.StatusCode = (int)HttpStatusCode.MovedPermanently;
                    context.Response.Headers["Location"] = UiIndexRoute.Replace("{document}", document);
                    return;
                }
            }

            if (IsRequestingAsyncApiUi(context.Request))
            {
                await RespondWithAsyncApiHtml(context.Response, _options.Middleware.Route);
                return;
            }

            {

                if (IsRequestingNamedAsyncApiUi(context.Request, out var document))
                {
                    await RespondWithAsyncApiHtml(context.Response, "/" + _options.Middleware.Route.Replace("{document}", document));
                    return;
                }
            }

            var documentName = GetDocumentName(context.Request);
            if (documentName == null)
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
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path.Value?.TrimEnd('/'), UiBaseRoute, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsRequestingNamedUiBase(HttpRequest request, out string documentName)
        {
            var template = TemplateParser.Parse(UiBaseRoute);

            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);

            documentName = null;
            var matching = matcher.TryMatch(request.Path, values);
            if (values.TryGetValue("document", out var temp))
            {
                documentName = temp.ToString();
            }

            return HttpMethods.IsGet(request.Method) && matching;
        }

        private string GetDocumentName(HttpRequest request)
        {
            var template = TemplateParser.Parse(_options.Middleware.UiBaseRoute + "{*wildcard}");
            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);

            string documentName = null;
            var matching = matcher.TryMatch(request.Path, values);
            if (values.TryGetValue("document", out var temp))
            {
                documentName = temp.ToString();
            }

            return documentName;
        }

        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, UiIndexRoute, StringComparison.OrdinalIgnoreCase);
        }


        private bool IsRequestingNamedAsyncApiUi(HttpRequest request, out string documentName)
        {
            var template = TemplateParser.Parse(UiIndexRoute);

            var values = new RouteValueDictionary();
            var matcher = new TemplateMatcher(template, values);

            documentName = null;
            var matching = matcher.TryMatch(request.Path, values);
            if (values.TryGetValue("document", out var temp))
            {
                documentName = temp.ToString();
            }

            return HttpMethods.IsGet(request.Method) && matching;
        }

        private string UiIndexRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') + "/index.html";

        private string UiBaseRoute => _options.Middleware.UiBaseRoute?.TrimEnd('/') ?? string.Empty;
    }
}