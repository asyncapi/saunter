using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class CorrelationId
    {
        public CorrelationId(string location)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; }

    }
}