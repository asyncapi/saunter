using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 
{
    /// <remarks>
    /// Can be either a <see cref="Schema"/> or <see cref="Reference"/> to a schema.
    /// </remarks>
    public interface ISchema { }

    /// <summary>
    /// The json-schema compatible definition of some type.
    /// </summary>
    public class Schema : ISchema
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }
        
        [JsonPropertyName("format")]
        public string Format { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
        
        [JsonPropertyName("minimum")]
        public decimal? Minimum { get; set; }
        
        [JsonPropertyName("maximum")]
        public decimal? Maximum { get; set; }
        
        [JsonPropertyName("maxLength")]
        public int? MaxLength { get; set; }
        
        [JsonPropertyName("minLength")]
        public int? MinLength { get; set; }
        
        [JsonPropertyName("minItems")]
        public int? MinItems { get; set; }
        
        [JsonPropertyName("maxItems")]
        public int? MaxItems { get; set; }
        
        [JsonPropertyName("uniqueItems")]
        public bool? UniqueItems { get; set; }
        
        [JsonPropertyName("pattern")]
        public string Pattern { get; set; }

        [JsonPropertyName("required")]
        public ISet<string> Required { get; set; }

        [JsonPropertyName("items")]
        public ISchema Items { get; set; }

        [JsonPropertyName("example")]
        public string Example { get; set; }

        [JsonPropertyName("enum")]
        public IList Enum { get; set; }

        [JsonPropertyName("properties")]
        public IDictionary<string, ISchema> Properties { get; set; }

        [JsonPropertyName("discriminator")]
        public string Discriminator { get; set; }

        [JsonPropertyName("oneOf")]
        public IEnumerable<ISchema> OneOf { get; set; }
    }
}