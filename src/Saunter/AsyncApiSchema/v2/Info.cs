using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class Info
    {
        public Info(string title, string version)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        [JsonPropertyName("title")]
        public string Title { get; }

        [JsonPropertyName("version")]
        public string Version { get;  }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("termsOfService")]
        public string? TermsOfService { get; set; }

        [JsonPropertyName("contact")]
        public Contact? Contact { get; set; }

        [JsonPropertyName("license")]
        public License? License { get; set; }
    }
}