using LEGO.AsyncAPI.Models;
using Saunter.Options;

namespace Saunter
{
    public interface IAsyncApiDocumentProvider
    {
        AsyncApiDocument GetDocument(string? documentName, AsyncApiOptions options);
    }
}
