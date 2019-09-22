namespace Saunter
{
    public interface IAsyncApiSchemaProvider
    {
        AsyncApiSchema.v2_0_0.AsyncApiSchema GetSchema();
    }
}