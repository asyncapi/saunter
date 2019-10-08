using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Message
    {
        [JsonProperty("headers")]
        public Reference Headers { get; set; }

        [JsonProperty("payload")]
        public Reference Payload { get; set; }

        [JsonProperty("correlationId")]
        public CorrelationId CorrelationId { get; set; }

        [JsonProperty("schemaFormat")]
        public string SchemaFormat { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public IList<Tag> Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        [JsonProperty("bindings")]
        public MessageBindings Bindings { get; set; }

        [JsonProperty("examples")]
        public IDictionary<string, object> Examples { get; set; }
    }
}