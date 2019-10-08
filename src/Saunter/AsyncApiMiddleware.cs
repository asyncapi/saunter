using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Saunter
{
    public class AsyncApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<AsyncApiOptions> _asyncApiOptionsAccessor;

        public AsyncApiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> asyncApiOptionsAccessor)
        {
            _next = next;
            _asyncApiOptionsAccessor = asyncApiOptionsAccessor;
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
            var asyncApiSchemaJson = JsonConvert.SerializeObject(
                asyncApiSchema, 
                Formatting.None,
                new JsonSerializerSettings
                    {
                       NullValueHandling = NullValueHandling.Ignore
                    });

            response.StatusCode = (int) HttpStatusCode.OK;
            response.ContentType = "application/json";

            await response.WriteAsync(asyncApiSchemaJson);
        }

        private bool IsRequestingAsyncApiSchema(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _asyncApiOptionsAccessor.Value.Route, StringComparison.OrdinalIgnoreCase);
        }
    }
}