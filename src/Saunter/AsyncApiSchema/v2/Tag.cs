using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class Tag
    {
        public Tag(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        public static implicit operator Tag(string s)
        {
            return new Tag(s);
        }
    }
}