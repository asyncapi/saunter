using System.Threading;
using System.Threading.Tasks;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Serialization
{
    public interface IAsyncApiDocumentSerializer
    {
        string ContentType { get; }

        string Serialize(AsyncApiDocument document);

        Task<AsyncApiDocument> DeserializeAsync(string data, CancellationToken cancellationToken);
    }
}
