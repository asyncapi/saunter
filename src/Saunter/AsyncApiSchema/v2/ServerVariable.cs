using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class ServerVariable
    {
        [JsonPropertyName("enum")]
        public IList<string>? Enum { get; set; }

        [JsonPropertyName("default")]
        public string? Default { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("examples")]
        public IList<string>? Examples { get; set; }
    }
}