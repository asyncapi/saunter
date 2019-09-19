using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class Schema : NJsonSchema.JsonSchema
    {
        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        [JsonProperty("deprecated")]
        public bool Deprecated { get; set; }
    }
}