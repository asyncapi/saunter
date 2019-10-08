using Saunter.AsyncApiSchema.v2;

namespace Saunter
{
    public interface IAsyncApiDocumentProvider
    {
        AsyncApiDocument GetDocument();
    }
}