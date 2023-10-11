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
        var contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.JsonSchema);
        return JsonSchemaSerialization.ToJson(document, SchemaType.JsonSchema, contractResolver, Formatting.Indented);
    }

    public async Task<AsyncApiDocument> DeserializeAsync(string data, CancellationToken cancellationToken)
    {
        var contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.JsonSchema);
        return await JsonSchemaSerialization.FromJsonAsync<AsyncApiDocument>(data, SchemaType.JsonSchema, null, document =>
        {
            var schemaResolver = new AsyncApiSchemaResolver(document, new AsyncApiSchemaOptions());
            return new JsonReferenceResolver(schemaResolver);
        }, contractResolver, cancellationToken).ConfigureAwait(false);
    }
}
