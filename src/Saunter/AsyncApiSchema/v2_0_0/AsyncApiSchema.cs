using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2_0_0
{
    public class AsyncApiSchema
    {
        public AsyncApiSchema(AsyncApiVersionString asyncapi, Info info, Channels channels)
        {
            AsyncApi = asyncapi ?? throw new ArgumentNullException(nameof(asyncapi));
            Info = info ?? throw new ArgumentNullException(nameof(info));
            Channels = channels ?? throw new ArgumentNullException(nameof(channels));
        }

        [JsonProperty("asyncapi")]
        [JsonConverter(typeof(AsyncApiVersionString.JsonConverter))]
        public AsyncApiVersionString AsyncApi { get; }
        
        [JsonProperty("id")]
        [JsonConverter(typeof(Identifier.JsonConverter))]
        public Identifier Id { get; set; }

        [JsonProperty("info")]
        public Info Info { get; }

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