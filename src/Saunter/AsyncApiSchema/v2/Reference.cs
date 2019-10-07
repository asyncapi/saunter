using System;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Reference
    {
        public Reference(string id, ReferenceType type)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
        
        [JsonIgnore]
        public string Id { get; set; }

        [JsonIgnore]
        public ReferenceType Type { get; set; }

        [JsonProperty("$ref")]
        public string Ref => Type.GetReferencePath(Id);
    }

    public class ReferenceType
    {
        public static readonly ReferenceType Schema = new ReferenceType("#/components/schemas/{0}");

     
        
        private string _format;
        
        private ReferenceType(string format)
        {
            _format = format;
        }

        public string GetReferencePath(string id) => string.Format(_format, id);

    }
}