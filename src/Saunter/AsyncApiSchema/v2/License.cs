using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2 {
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