using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("format")]
        public string Format { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("minimum")]
        public decimal? Minimum { get; set; }
        
        [JsonProperty("maximum")]
        public decimal? Maximum { get; set; }
        
        [JsonProperty("maxLength")]
        public int? MaxLength { get; set; }
        
        [JsonProperty("minLength")]
        public int? MinLength { get; set; }
        
        [JsonProperty("minItems")]
        public int? MinItems { get; set; }
        
        [JsonProperty("maxItems")]
        public int? MaxItems { get; set; }
        
        [JsonProperty("uniqueItems")]
        public bool? UniqueItems { get; set; }
        
        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("required")]
        public ISet<string> Required { get; set; }

        [JsonProperty("items")]
        public ISchema Items { get; set; }

        [JsonProperty("example")]
        public string Example { get; set; }

        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }

        [JsonProperty("properties")]
        public IDictionary<string, ISchema> Properties { get; set; }
    }
    
    /// <summary>
    /// A reference to a <see cref="Schema"/> within the asyncapi document. 
    /// </summary>
    public class Reference : ISchema
    {
        public Reference(string id, ReferenceType type)
        {
            _id = id ?? throw new ArgumentNullException(nameof(id));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        private readonly string _id;
        private readonly ReferenceType _type;

        [JsonProperty("$ref")]
        public string Ref => _type.GetReferencePath(_id);
    }

    /// <summary>
    /// The type of a <see cref="Reference"/>. Determines where the reference will be located inside the asyncapi document.
    /// </summary>
    public class ReferenceType
    {
        public static readonly ReferenceType Schema = new ReferenceType(nameof(Schema), "#/components/schemas/{0}");

        private ReferenceType(string name, string format)
        {
            Name = name;
            _format = format;
        }

        private string _format;
        
        public string Name { get; }
        
        public string GetReferencePath(string id) => string.Format(_format, id);
    }
}