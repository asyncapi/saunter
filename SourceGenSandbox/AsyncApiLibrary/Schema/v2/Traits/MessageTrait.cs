using AsyncApiLibrary.Schema.v2.Bindings;

using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2.Traits;

public class MessageTrait
{
    /// <summary>
    /// Schema definition of the application headers.
    /// Schema MUST be of type "object". It MUST NOT define the protocol headers.
    /// </summary>
    public JSchema? Headers { get; set; }

    /// <summary>
    /// Definition of the correlation ID used for message tracing or matching.
    /// </summary>
    public CorrelationId? CorrelationId { get; set; }

    /// <summary>
    /// A string containing the name of the schema format/language used to define
    /// the message payload. If omitted, implementations should parse the payload as a Schema object.
    /// </summary>
    public string? SchemaFormat { get; set; }

    /// <summary>
    /// The content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// When omitted, the value MUST be the one specified on the defaultContentType field.
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
    /// A verbose explanation of the message.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control.
    /// Tags can be used for logical grouping of messages.
    /// </summary>
    public ISet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this message.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol
    /// and the values describe protocol-specific definitions for the message.
    /// </summary>
    public MessageBindings? Bindings { get; set; }

    /// <summary>
    /// An array with examples of valid message objects.
    /// </summary>
    public IList<IDictionary<string, object>>? Examples { get; set; }
}