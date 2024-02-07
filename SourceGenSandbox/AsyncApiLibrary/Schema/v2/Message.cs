using AsyncApiLibrary.Schema.v2.Bindings;
using AsyncApiLibrary.Schema.v2.Traits;

using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2;

public class Messages
{
    public List<Message> OneOf { get; set; } = [];
}

public class Message
{
    /// <summary>
    /// Unique string used to identify the message. The id MUST be unique among all messages
    /// described in the API. The messageId value is case-sensitive. Tools and libraries MAY
    /// use the messageId to uniquely identify a message, therefore, it is RECOMMENDED to
    /// follow common programming naming conventions.
    /// </summary>
    public string? MessageId { get; set; }

    /// <summary>
    /// Schema definition of the application headers. Schema MUST be of type “object”.
    /// It MUST NOT define the protocol headers.
    /// TODO: add ref type
    /// </summary>
    public JSchema? Headers { get; set; }

    /// <summary>
    /// Definition of the message payload. It can be of any type but defaults to Schema object.
    /// TODO: add any type, or not...
    /// </summary>
    public JSchema? Payload { get; set; }

    /// <summary>
    /// Definition of the correlation ID used for message tracing or matching.
    /// </summary>
    public CorrelationId? CorrelationId { get; set; }

    /// <summary>
    /// A string containing the name of the schema format used to define the message payload.
    /// If omitted, implementations should parse the payload as a Schema object.
    /// </summary>
    public string? SchemaFormat { get; set; }

    /// <summary>
    /// The content type to use when encoding/decoding a message’s payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// A machine-friendly name for the message.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// A human-friendly title for the message.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// A short summary of what the message is about.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the message. CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control. Tags can be used for logical grouping of messages.
    /// </summary>
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this message.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
    /// </summary>
    public MessageBindings? Bindings { get; set; }

    /// <summary>
    /// An array with examples of valid message objects.
    /// </summary>
    public List<MessageExample> Examples { get; set; } = [];

    /// <summary>
    /// A list of traits to apply to the message object.
    /// Traits MUST be merged into the message object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// The resulting object MUST be a valid Message Object.
    /// </summary>
    public List<MessageTrait> Traits { get; set; } = [];
}

public class MessageExample
{
    /// <summary>
    /// A machine friendly name for the example.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// A short summary of what the example is about.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Example of headers that will be included in the message.
    /// </summary>
    public Dictionary<string, object>? Headers { get; set; }

    /// <summary>
    /// Example message payload.
    /// </summary>
    public object? Payload { get; set; }
}