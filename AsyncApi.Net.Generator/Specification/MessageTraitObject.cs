using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a trait that MAY be applied to a Message Object. This object MAY contain any property from the Message Object, except payload and traits.
/// </summary>
public class MessageTraitObject
{
    /// <summary>
    /// Gets or sets the schema definition of the application headers. Schema MUST be a map of key-value pairs.
    /// If this is a Schema Object, then the schemaFormat will be assumed to be "application/vnd.aai.asyncapi+json;version=`asyncapi`" where the version is equal to the AsyncAPI Version String.
    /// </summary>
    public MultiFormatSchemaObject Headers { get; set; }

    /// <summary>
    /// Gets or sets the definition of the correlation ID used for message tracing or matching.
    /// </summary>
    public CorrelationIdObject CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// When omitted, the value MUST be the one specified on the defaultContentType field.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets a machine-friendly name for the message.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a human-friendly title for the message.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a short summary of what the message is about.
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// Gets or sets a verbose explanation of the message. CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a list of tags for logical grouping and categorization of messages.
    /// </summary>
    public TagsObject Tags { get; set; }

    /// <summary>
    /// Gets or sets additional external documentation for this message.
    /// </summary>
    public ExternalDocumentationObject ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
    /// </summary>
    public MessageBindingsObject Bindings { get; set; }

    /// <summary>
    /// Gets or sets a list of examples.
    /// </summary>
    public List<MessageExampleObject> Examples { get; set; }
}
