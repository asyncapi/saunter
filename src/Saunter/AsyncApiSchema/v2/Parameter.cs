using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class Parameter 
    {
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("schema")]
        public Schema? Schema { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }
    }
}