using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class Parameter 
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("schema")]
        public ISchema Schema { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }
    }
}