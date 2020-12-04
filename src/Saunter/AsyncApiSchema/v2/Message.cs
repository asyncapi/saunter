using NJsonSchema;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    /// <remarks>
    /// Message can either be a list of messages 'oneOf', or a message. 
    /// </remarks>
    public interface IMessage {}

    public class Messages : IMessage
    {
        [JsonPropertyName("oneOf")]
        public List<Message> OneOf { get; set; } = new List<Message>();
    }
    
    public class Message : IMessage
    {
        /// <summary>
        /// Schema definition of the application headers. Schema MUST be of type “object”.
        /// It MUST NOT define the protocol headers.
        /// </summary>
        [JsonPropertyName("headers")]
        public JsonSchema Headers { get; set; }

        /// <summary>
        /// Definition of the message payload. It can be of any type but defaults to Schema object.
        /// </summary>
        [JsonPropertyName("payload")]
        public JsonSchema Payload { get; set; }

        /// <summary>
        /// Definition of the correlation ID used for message tracing or matching.
        /// </summary>
        [JsonPropertyName("correlationId")]
        public CorrelationId CorrelationId { get; set; }

        /// <summary>
        /// A string containing the name of the schema format used to define the message payload.
        /// If omitted, implementations should parse the payload as a Schema object.
        /// </summary>
        [JsonPropertyName("schemaFormat")]
        public string SchemaFormat { get; set; }

        /// <summary>
        /// The content type to use when encoding/decoding a message’s payload.
        /// The value MUST be a specific media type (e.g. application/json).
        /// </summary>
        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        /// <summary>
        /// A machine-friendly name for the message.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        /// <summary>
        /// A verbose explanation of the message. CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// A list of tags for API documentation control. Tags can be used for logical grouping of messages.
        /// </summary>
        [JsonPropertyName("tags")]
        public ISet<Tag> Tags { get; set; } = new HashSet<Tag>();

        /// <summary>
        /// Additional external documentation for this message.
        /// </summary>
        [JsonPropertyName("externalDocs")]
        public ExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// A free-form map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
        /// </summary>
        [JsonPropertyName("bindings")]
        public MessageBindings Bindings { get; set; }

        /// <summary>
        /// An array with examples of valid message objects.
        /// </summary>
        [JsonPropertyName("examples")]
        public IList<IDictionary<string, object>> Examples { get; set; } = new List<IDictionary<string, object>>();

        /// <summary>
        /// A list of traits to apply to the message object.
        /// Traits MUST be merged into the message object using the JSON Merge Patch algorithm in the same order they are defined here.
        /// The resulting object MUST be a valid Message Object.
        /// </summary>
        [JsonPropertyName("traits")]
        public IList<IMessageTrait> Traits { get; set; } = new List<IMessageTrait>();
    }
}