using System.Net;
using System.Threading.Tasks;
using LEGO.AsyncAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Saunter.Options;

namespace Saunter.DocumentMiddleware
{
    internal class AsyncApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAsyncApiDocumentProvider _asyncApiDocumentProvider;
        private readonly AsyncApiOptions _options;

        public AsyncApiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IAsyncApiDocumentProvider asyncApiDocumentProvider)
        {
            _next = next;
            _asyncApiDocumentProvider = asyncApiDocumentProvider;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!IsRequestingAsyncApiSchema(context.Request))
            {
                await _next(context);
                return;
            }

            if (context.TryGetDocument(out var documentName) && !_options.NamedApis.TryGetValue(documentName, out _))
            {
                await _next(context);
                return;
            }

            var asyncApiSchema = _asyncApiDocumentProvider.GetDocument(documentName, _options);

            await RespondWithAsyncApiSchemaJson(context.Response, asyncApiSchema);
        }

        private static async Task RespondWithAsyncApiSchemaJson(HttpResponse response, AsyncApiDocument asyncApiSchema)
        {
            var asyncApiSchemaJson = asyncApiSchema.SerializeAsJson(LEGO.AsyncAPI.AsyncApiVersion.AsyncApi2_0);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/json";

            await response.WriteAsync(asyncApiSchemaJson);
        }

        private bool IsRequestingAsyncApiSchema(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method) && request.Path.IsMatchingRoute(_options.Middleware.Route);
        }
    }
}
