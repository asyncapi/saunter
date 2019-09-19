using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class Info
    {
        public Info(string title, string version)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("termsOfService")]
        public string TermsOfService { get; set; }

        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        [JsonProperty("license")]
        public License License { get; set; }
    }
}