using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Tag
    {
        public Tag(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [JsonProperty("name")]
        public string Name { get; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        public static implicit operator Tag(string s)
        {
            return new Tag(s);
        }
    }
}