using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Can be either a <see cref="MessageTrait"/> or <see cref="Reference"/> to a message trait.
    /// </summary>
    public interface IMessageTrait { }
    
    public class MessageTrait : IMessageTrait
    {
        [JsonPropertyName("headers")]
        public ISchema? Headers { get; set; }

        [JsonPropertyName("correlationId")]
        public CorrelationId? CorrelationId { get; set; }

        [JsonPropertyName("schemaFormat")]
        public string? SchemaFormat { get; set; }

        [JsonPropertyName("contentType")]
        public string? ContentType { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tags")]
        public ISet<Tag>? Tags { get; set; } 
            
        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation? ExternalDocs { get; set; }

        [JsonPropertyName("bindings")]
        public MessageBindings? Bindings { get; set; }

        [JsonPropertyName("examples")]
        public IDictionary<string, object>? Examples { get; set; }
    }
}