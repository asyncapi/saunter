using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Utils
{
    public class JsonAsyncApiDocumentSerializer : IAsyncApiDocumentSerializer
    {
        private readonly IOptions<AsyncApiOptions> _options;

        public JsonAsyncApiDocumentSerializer(IOptions<AsyncApiOptions> options)
        {
            _options = options;
        }

        public string Serialize(AsyncApiDocument document)
        {
            return JsonConvert.SerializeObject(
                document,
                _options.Value.JsonSerializerSettings
            );
        }

        public string ContentType => "application/json";
    }
}