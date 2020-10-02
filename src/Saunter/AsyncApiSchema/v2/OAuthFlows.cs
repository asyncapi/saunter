using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class OAuthFlows
    {
        [JsonPropertyName("implicit")]
        public OAuthFlow Implicit { get; set; }

        [JsonPropertyName("password")]
        public OAuthFlow Password { get; set; }

        [JsonPropertyName("clientCredentials")]
        public OAuthFlow ClientCredentials { get; set; }

        [JsonPropertyName("authorizationCode")]
        public OAuthFlow AuthorizationCode { get; set; }

    }
}