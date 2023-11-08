using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

using Saunter.Serialization;
using Saunter.Utils;

namespace Saunter;

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
        if (!IsRequestingAsyncApiSchema(context.Request))
        {
            await _next(context);
            return;
        }

        AsyncApiSchema.v2.AsyncApiDocument? prototype = _options.AsyncApi;

        if (context.TryGetDocument(out string? documentName) && documentName is not null && documentName != "{document}" && !_options.NamedApis.TryGetValue(documentName, out prototype))
        {
            await _next(context);
            return;
        }

        AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema = _asyncApiDocumentProvider.GetDocument(_options, prototype);

        await RespondWithAsyncApiSchemaJson(context.Response, asyncApiSchema, _asyncApiDocumentSerializer);
    }

    private static async Task RespondWithAsyncApiSchemaJson(HttpResponse response, AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema, IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
    {
        string asyncApiSchemaJson = asyncApiDocumentSerializer.Serialize(asyncApiSchema);
        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = asyncApiDocumentSerializer.ContentType;

        await response.WriteAsync(asyncApiSchemaJson);
    }

    private bool IsRequestingAsyncApiSchema(HttpRequest request)
    {
        return HttpMethods.IsGet(request.Method) && (
            request.Path.IsMatchingRoute(_options.Middleware.Route) ||
            request.Path.IsMatchingRoute(_options.Middleware.Route.Replace("/{document}", null)));
    }
}