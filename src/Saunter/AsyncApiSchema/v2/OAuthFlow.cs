using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class OAuthFlow
    {
        [JsonPropertyName("authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [JsonPropertyName("tokenUrl")]
        public string TokenUrl { get; set; }

        [JsonPropertyName("refreshUrl")]
        public string RefreshUrl { get; set; }

        [JsonPropertyName("scopes")]
        public IDictionary<string,string> Scopes { get; set; }
    }
}