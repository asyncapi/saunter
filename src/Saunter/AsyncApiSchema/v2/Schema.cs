using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2 
{
    [JsonConverter(typeof(SchemaJsonConverter))]
    public class Schema
    {
        /// <remarks>
        /// Note serialization is handled differently if the reference is set, see <see cref="SchemaJsonConverter"/>.
        /// </remarks>
        [JsonIgnore]
        public Reference Reference { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("maximum")]
        public decimal? Maximum { get; set; }

        [JsonProperty("minimum")]
        public decimal? Minimum { get; set; }

        [JsonProperty("maxLength")]
        public int? MaxLength { get; set; }

        [JsonProperty("minLength")]
        public int? MinLength { get; set; }

        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        [JsonProperty("required")]
        public ISet<string> Required { get; set; } = new HashSet<string>();

        [JsonProperty("items")]
        public Reference Items { get; set; }

        [JsonProperty("example")]
        public string Example { get; set; }

        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }

    }
    
    public class SchemaJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Schema);
        }
        
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is Schema schema))
            {
                return;
            }

            if (schema.Reference != null)
            {
                serializer.Serialize(writer, schema.Reference);
            }
            else
            {
                serializer.Serialize(writer, schema);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException("This JsonConverter does not support deserializing");
        }
    }
}