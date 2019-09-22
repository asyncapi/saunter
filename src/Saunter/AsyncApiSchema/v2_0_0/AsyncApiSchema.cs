using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Saunter.AsyncApiSchema.v2_0_0
{
    public class AsyncApiSchema
    {
        public AsyncApiSchema()
        {
            AsyncApi = AsyncApiVersionString.V2_0_0;
        }
        
        [JsonProperty("asyncapi")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AsyncApiVersionString AsyncApi { get; set; }
        
        [JsonProperty("id")]
        [JsonConverter(typeof(Identifier.JsonConverter))]
        public Identifier Id { get; set; }

        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("servers")]
        public Servers Servers { get; set; }

        [JsonProperty("defaultContentType")]
        public string DefaultContentType { get; set; }

        [JsonProperty("channels")]
        public Channels Channels { get; }

        [JsonProperty("components")]
        public Components Components { get; set; }

        [JsonProperty("tags")]
        public ISet<Tag> Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }
    }
}