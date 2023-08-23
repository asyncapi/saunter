using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Saunter.Serialization;

namespace Saunter;

internal sealed class AsyncApiMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAsyncApiDocumentProvider _asyncApiDocumentProvider;
    private readonly IAsyncApiDocumentSerializer _asyncApiDocumentSerializer;
    private readonly AsyncApiOptions _options;

    public AsyncApiMiddleware(
        RequestDelegate next,
        IOptions<AsyncApiOptions> options,
        IAsyncApiDocumentProvider asyncApiDocumentProvider,
        IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
    {
        _next = next;
        _asyncApiDocumentProvider = asyncApiDocumentProvider;
        _asyncApiDocumentSerializer = asyncApiDocumentSerializer;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.IsRequestingAsyncApiDocument(_options))
        {
            await _next(context);
            return;
        }

        var prototype = _options.AsyncApi;
        if (context.TryGetDocument(_options, out var documentName) && !_options.NamedApis.TryGetValue(documentName, out prototype))
        {
            await _next(context);
            return;
        }

        var asyncApiSchema = _asyncApiDocumentProvider.GetDocument(_options, prototype);

        await RespondWithAsyncApiSchemaJsonAsync(context.Response, asyncApiSchema, _asyncApiDocumentSerializer);
    }

    private static async Task RespondWithAsyncApiSchemaJsonAsync(
        HttpResponse response,
        AsyncApiSchema.v2.AsyncApiDocument asyncApiSchema,
        IAsyncApiDocumentSerializer asyncApiDocumentSerializer)
    {
        var asyncApiSchemaJson = asyncApiDocumentSerializer.Serialize(asyncApiSchema);
        response.StatusCode = (int)HttpStatusCode.OK;
        response.ContentType = asyncApiDocumentSerializer.ContentType;

        await response.WriteAsync(asyncApiSchemaJson);
    }
}