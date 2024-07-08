using LEGO.AsyncAPI.Models;
using Saunter.Options;

namespace Saunter
{
    public interface IAsyncApiDocumentProvider
    {
        AsyncApiDocument GetDocument(AsyncApiOptions options, AsyncApiDocument prototype);
    }
}
