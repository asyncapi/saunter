#if !NETSTANDARD2_0

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Saunter.Serialization;

namespace Saunter
{
    public class AsyncApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAsyncApiDocumentProvider _asyncApiDocumentProvider;
        private readonly AsyncApiOptions _options;

        public AsyncApiMiddleware(RequestDelegate next, IOptionsSnapshot<AsyncApiOptions> options, IAsyncApiDocumentProvider asyncApiDocumentProvider, string apiName = null)
        {
            _next = next;
            _asyncApiDocumentProvider = asyncApiDocumentProvider;

            if (apiName == null)
            {
                _options = options.Value;
            }
            else
            {
                _options = options.Get(apiName);
            }
        }

        public async Task Invoke(HttpContext context, IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
        {
            if (!IsRequestingAsyncApiSchema(context.Request))
            {
                await _next(context);
                return;
            }

            var asyncApiSchema = _asyncApiDocumentProvider.GetDocument(_options);

            await RespondWithAsyncApiSchemaJson(context.Response, asyncApiSchema, asyncApiDocumentSerializer, _options);
        }

        private async Task RespondWithAsyncApiSchemaJson(HttpResponse response, AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema, IAsyncApiDocumentSerializer asyncApiDocumentSerializer, AsyncApiOptions options)
        {
            var asyncApiSchemaJson = asyncApiDocumentSerializer.Serialize(asyncApiSchema, options);
            response.StatusCode = (int)HttpStatusCode.OK;
            response.ContentType = asyncApiDocumentSerializer.ContentType;

            await response.WriteAsync(asyncApiSchemaJson);
        }

        private bool IsRequestingAsyncApiSchema(HttpRequest request)
        {
            return HttpMethods.IsGet(request.Method)
                   && string.Equals(request.Path, _options.Middleware.Route, StringComparison.OrdinalIgnoreCase);
        }
    }
}

#endif