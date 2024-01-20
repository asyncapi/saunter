using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a specific operation.
/// </summary>
public class OperationObject
{
    /// <summary>
    /// Gets or sets whether the application will send or receive messages.
    /// Use "send" when it's expected that the application will send a message to the given channel,
    /// and "receive" when the application should expect receiving messages from the given channel.
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets a reference to the definition of the channel in which this operation is performed.
    /// </summary>
    public ReferenceObject Channel { get; set; }

    /// <summary>
    /// Gets or sets a human-friendly title for the operation.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a short summary of what the operation is about.
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// Gets or sets a verbose explanation of the operation.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a declaration of which security schemes are associated with this operation.
    /// Only one of the security scheme objects must be satisfied to authorize an operation.
    /// In cases where Server Security also applies, it must also be satisfied.
    /// </summary>
    public List<SecuritySchemeObject> Security { get; set; }

    /// <summary>
    /// Gets or sets a list of tags for logical grouping and categorization of operations.
    /// </summary>
    public TagsObject Tags { get; set; }

    /// <summary>
    /// Gets or sets additional external documentation for this operation.
    /// </summary>
    public ExternalDocumentationObject ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific definitions for the operation.
    /// </summary>
    public Dictionary<string, OperationBindingsObject> Bindings { get; set; }

    /// <summary>
    /// Gets or sets a list of traits to apply to the operation object.
    /// Traits must be merged using traits merge mechanism.
    /// The resulting object must be a valid Operation Object.
    /// </summary>
    public List<OperationTraitObject> Traits { get; set; }

    /// <summary>
    /// Gets or sets a list of supported Message Objects that can be processed by this operation.
    /// It must contain a subset of the messages defined in the channel referenced in this operation,
    /// and must not point to a subset of message definitions located in the Messages Object in the Components Object or anywhere else.
    /// Every message processed by this operation must be valid against one, and only one, of the message objects referenced in this list.
    /// </summary>
    public List<ReferenceObject> Messages { get; set; }

    /// <summary>
    /// Gets or sets the definition of the reply in a request-reply operation.
    /// </summary>
    public OperationReplyObject Reply { get; set; }
}
