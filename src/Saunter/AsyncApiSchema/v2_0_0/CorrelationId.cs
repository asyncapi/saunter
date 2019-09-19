using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class CorrelationId
    {
        public CorrelationId(string location)
        {
            Location = location ?? throw new ArgumentNullException(nameof(location));
        }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("location")]
        public string Location { get; }

    }
}