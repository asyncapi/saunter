using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Message
    {
        [JsonProperty("headers")]
        public ISchema Headers { get; set; }

        [JsonProperty("payload")]
        public ISchema Payload { get; set; }

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
        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        [JsonProperty("bindings")]
        public MessageBindings Bindings { get; set; }

        [JsonProperty("examples")]
        public IList<IDictionary<string, object>> Examples { get; set; } = new List<IDictionary<string, object>>();

        [JsonProperty("traits")]
        public IList<IMessageTrait> Traits { get; set; } = new List<IMessageTrait>();
    }
}