#if NETCOREAPP3_0
using System;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiUiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<AsyncApiOptions> _options;
        private readonly StaticFileMiddleware _staticFileMiddleware;
        private const string EmbeddedFileNamespace = "Saunter.wwwroot";

        public AsyncApiUiMiddleware(
            RequestDelegate next, 
            IOptions<AsyncApiOptions> options,
            IWebHostEnvironment IWebHostEnvironment,
            ILoggerFactory loggerFactory
            )
        {
            _next = next;
            _options = options;
            _staticFileMiddleware = CreateStaticFileMiddleware(
                next,
                IWebHostEnvironment,
                loggerFactory);
        }

        public async Task Invoke(HttpContext context, IAsyncApiDocumentProvider asyncApiDocumentProvider)
        {
            if (!IsRequestingAsyncApiUi(context.Request) && ! IsRequestingAsyncApiUiAssets(context.Request))
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
            
            await _staticFileMiddleware.Invoke(context);
        }

        private async Task RespondWithAsyncApiHtml(HttpResponse response, AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema)
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
                        new InterfaceImplementationConverter(),
                    },
                }
            );
            
            HttpClient client = new HttpClient
            {
                // TODO: Fix hardcoding
                BaseAddress = new Uri("http://localhost:83/html/generate")
            };

            var asyncApiUiHtml = await client.PostAsync("", new StringContent(asyncApiSchemaJson));


            response.StatusCode = (int) HttpStatusCode.OK;
            response.ContentType = MediaTypeNames.Text.Html;

            await response.WriteAsync(await asyncApiUiHtml.Content.ReadAsStringAsync());
        }
        
        private StaticFileMiddleware CreateStaticFileMiddleware(
            RequestDelegate next,
            IWebHostEnvironment hostingEnv,
            ILoggerFactory loggerFactory)
        {
            var staticFileOptions = new StaticFileOptions
            {
                RequestPath = "/asyncapi/ui",
                FileProvider = new EmbeddedFileProvider(typeof(AsyncApiUiMiddleware).GetTypeInfo().Assembly, EmbeddedFileNamespace),
            };

            return new StaticFileMiddleware(next, hostingEnv, Options.Create(staticFileOptions), loggerFactory);
        }


        private bool IsRequestingAsyncApiUiAssets(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) &&
                   request.Path.ToString().ToLower().StartsWith("/asyncapi/ui");
        }
        
        private bool IsRequestingAsyncApiUi(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _options.Value.Middleware.UiRoute, StringComparison.OrdinalIgnoreCase);
        }
        
    }
}
#endif