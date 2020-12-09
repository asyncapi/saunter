using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class OAuthFlow
    {
        public OAuthFlow(string authorizationUrl, string tokenUrl, IDictionary<string, string> scopes)
        {
            AuthorizationUrl = authorizationUrl ?? throw new ArgumentNullException(nameof(authorizationUrl));
            TokenUrl = tokenUrl ?? throw new ArgumentNullException(nameof(tokenUrl));
            Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
        }

        [JsonPropertyName("authorizationUrl")]
        public string AuthorizationUrl { get; }

        [JsonPropertyName("tokenUrl")]
        public string TokenUrl { get; }

        [JsonPropertyName("refreshUrl")]
        public string? RefreshUrl { get; set; }

        [JsonPropertyName("scopes")]
        public IDictionary<string,string> Scopes { get; }
    }
}