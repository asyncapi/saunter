using LEGO.AsyncAPI.Models;

public interface IDocumentFilter
{
    void Apply(AsyncApiDocument document, DocumentFilterContext context);
}
