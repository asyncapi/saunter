using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class License
    {
        public License(string name)
        {
            Name = name;
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}