using Newtonsoft.Json;
using NJsonSchema;
using NJsonSchema.Infrastructure;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.SchemaGeneration;
using System.Threading;
using System.Threading.Tasks;

namespace Saunter.Serialization
{
    public class NewtonsoftAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
    {
        public string ContentType => "application/json";

        public string Serialize(AsyncApiDocument document)
        {
            var contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.OpenApi3);
            return JsonSchemaSerialization.ToJson(document, SchemaType.OpenApi3, contractResolver, Formatting.Indented);
        }

        public async Task<AsyncApiDocument> DeserializeAsync(string data, CancellationToken cancellationToken)
        {
            var contractResolver = JsonSchema.CreateJsonSerializerContractResolver(SchemaType.OpenApi3);
            return await JsonSchemaSerialization.FromJsonAsync<AsyncApiDocument>(data, SchemaType.OpenApi3, null, document =>
            {
                var schemaResolver = new AsyncApiSchemaResolver(document, new AsyncApiSchemaOptions());
                return new JsonReferenceResolver(schemaResolver);
            }, contractResolver, cancellationToken).ConfigureAwait(false);
        }
    }
}
