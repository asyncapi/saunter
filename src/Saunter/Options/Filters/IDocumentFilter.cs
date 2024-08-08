using LEGO.AsyncAPI.Models;

namespace Saunter.Options.Filters
{
    public interface IDocumentFilter
    {
        void Apply(AsyncApiDocument document, DocumentFilterContext context);
    }
}
