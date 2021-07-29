using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Options;
using Saunter.Serialization;

namespace Saunter
{
    public class AsyncApiMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAsyncApiDocumentProvider _asyncApiDocumentProvider;
        private readonly IAsyncApiDocumentSerializer _asyncApiDocumentSerializer;
        private readonly AsyncApiOptions _options;

        public AsyncApiMiddleware(RequestDelegate next, IOptions<AsyncApiOptions> options, IAsyncApiDocumentProvider asyncApiDocumentProvider, IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
        {
            _next = next;
            _asyncApiDocumentProvider = asyncApiDocumentProvider;
            _asyncApiDocumentSerializer = asyncApiDocumentSerializer;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            string documentName = null;
            if (!IsRequestingAsyncApiSchema(context.Request) && !IsRequestingNamedApi(context.Request, out documentName))
            {
                await _next(context);
                return;
            }

            var prototype = _options.AsyncApi;
            if (documentName != null)
            {
                if (!_options.NamedApis.TryGetValue(documentName, out prototype))
                {
                    await _next(context);
                    return;
                }
            }

            var asyncApiSchema = _asyncApiDocumentProvider.GetDocument(_options, prototype);

            await RespondWithAsyncApiSchemaJson(context.Response, asyncApiSchema, _asyncApiDocumentSerializer, _options);
        }

        private static async Task RespondWithAsyncApiSchemaJson(HttpResponse response, AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema, IAsyncApiDocumentSerializer asyncApiDocumentSerializer, AsyncApiOptions options)
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

        private bool IsRequestingNamedApi(HttpRequest request, out string documentName)
        {
            var template = TemplateParser.Parse(_options.Middleware.Route);

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
    }
}