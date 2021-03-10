using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class ChannelItem
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("subscribe")]
        public Operation Subscribe { get; set; }

        [JsonPropertyName("publish")]
        public Operation Publish { get; set; }

        [JsonPropertyName("parameters")]
        public Parameters Parameters { get; set; }

        [JsonPropertyName("bindings")]
        public ChannelBindings Bindings { get; set; }
    }
}