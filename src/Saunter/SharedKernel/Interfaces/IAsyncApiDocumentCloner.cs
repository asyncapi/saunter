using LEGO.AsyncAPI.Models;

namespace Saunter.SharedKernel.Interfaces
{
    public interface IAsyncApiDocumentCloner
    {
        AsyncApiDocument CloneProtype(AsyncApiDocument prototype);
    }
}
