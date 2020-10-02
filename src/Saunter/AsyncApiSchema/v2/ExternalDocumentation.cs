using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class ExternalDocumentation
    {
        public ExternalDocumentation(string url)
        {
            Url = url ?? throw new ArgumentNullException(nameof(url));
        }

        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("url")]
        public string Url { get; }
    }
}