#if NETCOREAPP3_0
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiUiMiddleware
    {
        private const string EmbeddedFileNamespace = "Saunter.wwwroot";
        private readonly RequestDelegate _next;
        private readonly IOptions<AsyncApiOptions> _options;

        public AsyncApiUiMiddleware(
            RequestDelegate next,
            IOptions<AsyncApiOptions> options
        )
        {
            _next = next;
            _options = options;
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

            // TODO: Respond with proxied request
            await RespondWithProxiedResponse(context.Request, context.Response);
        }

        private async Task RespondWithAsyncApiHtml(HttpResponse response, AsyncApiDocument asyncApiSchema)
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

            var client = new HttpClient
            {
                // TODO: Fix hardcoding
                BaseAddress = new Uri("https://playground.asyncapi.io/html/generate")
            };

            var asyncApiUiHtml = await client.PostAsync("", new StringContent(asyncApiSchemaJson));


            response.StatusCode = (int) HttpStatusCode.OK;
            response.ContentType = MediaTypeNames.Text.Html;

            var responseString = await asyncApiUiHtml.Content.ReadAsStringAsync();
            
            await response.WriteAsync(RewriteContent(responseString));
        }

        private string RewriteContent(string content)
        {
            content = content.Replace(
                "text-xs uppercase text-grey mt-10 mb-4 font-thin", 
                "text-xs uppercase text-dark-grey mt-10 mb-4 font-thin");
            
            content = content.Replace(
                "text-grey text-sm", 
                "text-dark-grey text-sm");
            return content;
        }
        
        private async Task RespondWithProxiedResponse(HttpRequest request, HttpResponse response)
        {
            var client = new HttpClient
            {
                // TODO: Fix hardcoding
                BaseAddress = new Uri("https://playground.asyncapi.io/")
            };

            var proxiedResult = await client.GetAsync(
                $"/html/template/{request.Path.Value.Substring("/asyncapi/ui/".Length)}"
            );


            response.StatusCode = (int) HttpStatusCode.OK;

            var responseString = await proxiedResult.Content.ReadAsStringAsync();
            responseString = responseString.Replace("font-weight: lighter", "font-weight: normal");
            await response.WriteAsync(responseString);
        }

        private bool IsRequestingAsyncApiUiAssets(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) &&
                   request.Path.ToString().ToLower().StartsWith("/asyncapi/ui");
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