using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2 {
    public class OAuthFlow
    {
        [JsonProperty("authorizationUrl")]
        public string AuthorizationUrl { get; set; }

        [JsonProperty("tokenUrl")]
        public string TokenUrl { get; set; }

        [JsonProperty("refreshUrl")]
        public string RefreshUrl { get; set; }

        [JsonProperty("scopes")]
        public IDictionary<string,string> Scopes { get; set; }
    }
}