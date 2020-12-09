using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class Operation
    {
        [JsonPropertyName("operationId")]
        public string? OperationId { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tags")]
        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();

        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation? ExternalDocs { get; set; }

        [JsonPropertyName("bindings")]
        public OperationBindings? Bindings { get; set; }

        [JsonPropertyName("message")]
        public IMessage? Message { get; set; }

        [JsonPropertyName("traits")]
        public IList<IOperationTrait> Traits { get; set; } = new List<IOperationTrait>();
    }
}