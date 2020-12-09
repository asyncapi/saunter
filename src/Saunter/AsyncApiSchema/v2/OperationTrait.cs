using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Can be either an <see cref="OperationTrait"/> or a <see cref="Reference"/> to an operation trait.
    /// </summary>
    public interface IOperationTrait { }
    
    public class OperationTrait : IOperationTrait
    {
        [JsonPropertyName("operationId")]
        public string? OperationId { get; set; }

        [JsonPropertyName("summary")]
        public string? Summary { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tags")]
        public ISet<Tag>? Tags { get; set; }

        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation? ExternalDocs { get; set; }

        [JsonPropertyName("bindings")]
        public IDictionary<string, OperationBindings>? Bindings { get; set; }
    }
}