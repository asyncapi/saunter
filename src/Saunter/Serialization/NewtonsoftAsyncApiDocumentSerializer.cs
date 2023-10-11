using Newtonsoft.Json;

using NJsonSchema;
using NJsonSchema.Infrastructure;

using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.SchemaGeneration;

using System.Threading;
using System.Threading.Tasks;

namespace Saunter.Serialization;

public class NewtonsoftAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
{
    public string ContentType => "application/json";

    public string Serialize(AsyncApiDocument document)
    {
        PropertyRenameAndIgnoreSerializerContractResolver contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.JsonSchema);
        return JsonSchemaSerialization.ToJson(document, SchemaType.JsonSchema, contractResolver, Formatting.Indented);
    }

    public async Task<AsyncApiDocument> DeserializeAsync(string data, CancellationToken cancellationToken)
    {
        PropertyRenameAndIgnoreSerializerContractResolver contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.JsonSchema);
        return await JsonSchemaSerialization.FromJsonAsync<AsyncApiDocument>(data, SchemaType.JsonSchema, null, document =>
        {
            AsyncApiSchemaResolver schemaResolver = new(document, new AsyncApiSchemaOptions());
            return new JsonReferenceResolver(schemaResolver);
        }, contractResolver, cancellationToken).ConfigureAwait(false);
    }
}
