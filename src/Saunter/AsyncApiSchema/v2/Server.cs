using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class Server
    {
        public Server(string url, string protocol)
        {
            Url = url;
            Protocol = protocol;
        }

        [JsonPropertyName("url")]
        public string Url { get; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; }

        [JsonPropertyName("protocolVersion")]
        public string ProtocolVersion { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("variables")]
        public IDictionary<string, ServerVariable> Variables { get; set; }

        [JsonPropertyName("security")]
        public IList<SecurityRequirement> Security { get; set; }

        [JsonPropertyName("bindings")]
        public ServerBindings Bindings { get; set; }
    }
}