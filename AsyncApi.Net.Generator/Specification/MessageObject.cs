using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a message received on a given channel and operation.
/// </summary>
public class MessageObject
{
    public MessageObject(MultiFormatSchemaObject payload)
    {
        Payload = payload;
    }

    /// <summary>
    /// Gets or sets the schema definition of the application headers.
    /// </summary>
    public MultiFormatSchemaObject? Headers { get; set; }

    /// <summary>
    /// Gets or sets the definition of the message payload.
    /// </summary>
    public MultiFormatSchemaObject Payload { get; set; }

    /// <summary>
    /// Gets or sets the definition of the correlation ID used for message tracing or matching.
    /// </summary>
    public CorrelationIdObject? CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets the content type to use when encoding/decoding a message's payload.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Gets or sets a machine-friendly name for the message.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets a human-friendly title for the message.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a short summary of what the message is about.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets a verbose explanation of the message.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a list of tags for logical grouping and categorization of messages.
    /// </summary>
    public TagsObject Tags { get; set; } = new();

    /// <summary>
    /// Gets or sets additional external documentation for this message.
    /// </summary>
    public ExternalDocumentationObject? ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets a map where the keys describe the name of the protocol and the values describe
    /// protocol-specific definitions for the message.
    /// </summary>
    public MessageBindingsObject? Bindings { get; set; }

    /// <summary>
    /// Gets or sets a list of examples.
    /// </summary>
    public List<MessageExampleObject>? Examples { get; set; }

    /// <summary>
    /// Gets or sets a list of traits to apply to the message object.
    /// </summary>
    public List<MessageTraitObject>? Traits { get; set; }
}
