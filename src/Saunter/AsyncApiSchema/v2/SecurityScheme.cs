using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class SecurityScheme
    {
        public SecurityScheme(SecuritySchemeType type)
        {
            Type = type;
        }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public SecuritySchemeType Type { get; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("in")]
        public string In { get; set; }

        [JsonPropertyName("scheme")]
        public string Scheme { get; set; }

        [JsonPropertyName("bearerFormat")]
        public string BearerFormat { get; set; }

        [JsonPropertyName("flows")]
        public OAuthFlows Flows { get; set; }

        [JsonPropertyName("openIdConnectUrl")]
        public string OpenIdConnectUrl { get; set; }
    }
}