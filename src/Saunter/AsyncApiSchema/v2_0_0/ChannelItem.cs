using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class ChannelItem
    {
        [JsonProperty("$ref")]
        public string Ref { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("subscribe")]
        public Operation Subscribe { get; set; }

        [JsonProperty("publish")]
        public Operation Publish { get; set; }

        [JsonProperty("parameters")]
        public string Parameters { get; set; }

        [JsonProperty("bindings")]
        public ChannelBindings Bindings { get; set; } = new ChannelBindings();
    }
}