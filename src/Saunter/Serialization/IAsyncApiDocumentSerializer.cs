using Saunter.AsyncApiSchema.v2;

namespace Saunter.Serialization
{
    public interface IAsyncApiDocumentSerializer
    {
        string ContentType { get; }

        string Serialize(AsyncApiDocument document, AsyncApiOptions options);
    }
}