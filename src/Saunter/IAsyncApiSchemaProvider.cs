namespace Saunter
{
    public interface IAsyncApiSchemaProvider
    {
        AsyncApiSchema.v2.AsyncApiSchema GetSchema();
    }
}