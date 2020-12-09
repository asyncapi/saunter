using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class License
    {
        public License(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}