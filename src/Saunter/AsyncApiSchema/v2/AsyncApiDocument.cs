using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class AsyncApiDocument
    {
        public AsyncApiDocument()
        {

        }

        public AsyncApiDocument(Info info)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
        }

        
        [JsonPropertyName("asyncapi")]
        public AsyncApiVersionString AsyncApi { get; } = AsyncApiVersionString.v2;
        
        [JsonPropertyName("id")]
        public Identifier? Id { get; set; }

        [JsonPropertyName("info")]
        public Info Info { get; set;}

        [JsonPropertyName("servers")]
        public Servers Servers { get; } = new Servers();

        [JsonPropertyName("defaultContentType")]
        public string DefaultContentType { get; set; } = "application/json";

        [JsonPropertyName("channels")]
        public Channels Channels { get; set; } = new Channels();

        [JsonPropertyName("components")]
        public Components Components { get; } = new Components();

        [JsonPropertyName("tags")]
        public ISet<Tag> Tags { get; } = new HashSet<Tag>();

        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation? ExternalDocs { get; set; }
    }
}