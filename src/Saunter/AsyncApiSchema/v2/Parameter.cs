using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2 {
    public class Parameter 
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("schema")]
        public NJsonSchema.JsonSchema Schema { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}