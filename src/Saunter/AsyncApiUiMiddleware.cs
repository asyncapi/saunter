#if NETCOREAPP3_0
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiUiMiddleware
    {

        private const string GenerateApiPath = "html/generate";
        private const string PlaygroundAssetsPath = "/html/template/";

        private readonly RequestDelegate _next;
        private readonly IOptions<AsyncApiOptions> _options;
        private readonly HttpClient _client;
        private readonly IMemoryCache _memoryCache;

        public AsyncApiUiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options)
        {
            _next = next;
            _options = options;
            _client = new HttpClient
            {
                BaseAddress = new Uri(_options.Value.Middleware.PlaygroundBaseAddress)
            };
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public async Task Invoke(HttpContext context, IAsyncApiDocumentProvider asyncApiDocumentProvider)
        {
            if (!IsRequestingAsyncApiUi(context.Request) && !IsRequestingAsyncApiUiAssets(context.Request))
            {
                await _next(context);
                return;
            }

            if (IsRequestingAsyncApiUi(context.Request))
            {
                var asyncApiSchema = asyncApiDocumentProvider.GetDocument();
                await RespondWithAsyncApiHtml(context.Response, asyncApiSchema);
                return;
            }

            await RespondWithProxiedResponse(context.Request, context.Response);
        }

        private async Task RespondWithAsyncApiHtml(HttpResponse response, AsyncApiDocument asyncApiSchema)
        {
            if (!_memoryCache.TryGetValue<string>("playground_html", out var responseString))
            {
                var asyncApiSchemaJson = JsonSerializer.Serialize(
                    asyncApiSchema,
                    new JsonSerializerOptions
                    {
                        WriteIndented = false,
                        IgnoreNullValues = true,
                        Converters =
                        {
                            new DictionaryKeyToStringConverter(),
                            new InterfaceImplementationConverter()
                        }
                    }
                );

                var asyncApiUiHtml = await _client.PostAsync(GenerateApiPath,
                    new StringContent(asyncApiSchemaJson));

                responseString = await asyncApiUiHtml.Content.ReadAsStringAsync();
                _memoryCache.Set("playground_html", responseString);
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = MediaTypeNames.Text.Html;
            await response.WriteAsync(RewriteContent(responseString, _options.Value.Middleware.HtmlProxyRewrites));
        }

        private async Task RespondWithProxiedResponse(HttpRequest request, HttpResponse response)
        {
            if (!_memoryCache.TryGetValue<string>(request.Path, out var responseString))
            {
                // Strip off the path this middleware is hosted at
                var playgroundPath = request.Path.Value.Substring(_options.Value.Middleware.UiBaseRoute.Length);

                var proxiedResult = await _client.GetAsync(
                    $"{PlaygroundAssetsPath}{playgroundPath}"
                );

                responseString = await proxiedResult.Content.ReadAsStringAsync();
                _memoryCache.Set(request.Path, responseString);
            }

            response.StatusCode = (int)HttpStatusCode.OK;
            await response.WriteAsync(RewriteContent(responseString,
                _options.Value.Middleware.AssetsProxyRewrites));
        }

        private string RewriteContent(string content, IDictionary<string, string> contentRewrites)
        {
            foreach (var (key, value) in contentRewrites)
            {
                content = content.Replace(key, value);
            }
            return content;
        }

        private bool IsRequestingAsyncApiUiAssets(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) &&
                   request.Path.ToString().ToLower().StartsWith(_options.Value.Middleware.UiBaseRoute);
        }

        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _options.Value.Middleware.UiRoute,
                       StringComparison.OrdinalIgnoreCase);
        }
    }
}
#endif