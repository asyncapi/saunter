using AsyncApi.Net.Generator.AsyncApiSchema.v2;

namespace AsyncApi.Net.Generator;

public interface IAsyncApiDocumentProvider
{
    AsyncApiDocument GetDocument(AsyncApiOptions options, AsyncApiDocument prototype);
}