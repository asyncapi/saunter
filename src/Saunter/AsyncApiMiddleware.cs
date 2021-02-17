#if !NETSTANDARD2_0

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<AsyncApiOptions> _options;
        private readonly JsonAsyncApiDocumentSerializer _serializer = new JsonAsyncApiDocumentSerializer();

        public AsyncApiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context, IAsyncApiDocumentProvider asyncApiDocumentProvider)
        {
            if (!IsRequestingAsyncApiSchema(context.Request))
            {
                await _next(context);
                return;
            }

            var asyncApiSchema = asyncApiDocumentProvider.GetDocument();

            await RespondWithAsyncApiSchemaJson(context.Response, asyncApiSchema);
        }

        private async Task RespondWithAsyncApiSchemaJson(HttpResponse response, AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema)
        {
            var asyncApiSchemaJson = _serializer.Serialize(asyncApiSchema);

            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = "application/json";

            await response.WriteAsync(asyncApiSchemaJson);
        }

        private bool IsRequestingAsyncApiSchema(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _options.Value.Middleware.Route, StringComparison.OrdinalIgnoreCase);
        }
    }
}

#endif