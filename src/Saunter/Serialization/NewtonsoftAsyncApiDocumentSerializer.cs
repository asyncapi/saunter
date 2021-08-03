using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Serialization
{
    public class NewtonsoftAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
    {
        public string ContentType => "application/json";

        public string Serialize(AsyncApiDocument document, AsyncApiOptions options)
        {
            return JsonConvert.SerializeObject(document, options.JsonSchemaGeneratorSettings.ActualSerializerSettings);
        }
    }
}
