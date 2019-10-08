using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Operation
    {
        [JsonProperty("operationId")]
        public string OperationId { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("tags")]
        public IList<Tag> Tags { get; set; }

        [JsonProperty("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        [JsonProperty("bindings")]
        public IDictionary<string, OperationBindings> Bindings { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }
}