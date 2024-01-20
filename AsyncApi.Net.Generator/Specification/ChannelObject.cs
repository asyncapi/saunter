using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a shared communication channel.
/// </summary>
public class ChannelObject
{
    /// <summary>
    /// Gets or sets an optional string representation of this channel's address.
    /// </summary>
    /// <remarks>
    /// An optional string representation of this channel's address. The address is typically the
    /// "topic name", "routing key", "event type", or "path". When `null` or absent, it MUST be
    /// interpreted as unknown. This is useful when the address is generated dynamically at runtime
    /// or can't be known upfront. It MAY contain Channel Address Expressions. Query parameters and
    /// fragments SHALL NOT be used, instead use bindings to define them.
    /// </remarks>
    public string? Address { get; set; }

    /// <summary>
    /// Gets or sets a map of the messages that will be sent to this channel by any application at any time.
    /// </summary>
    /// <remarks>
    /// A map of the messages that will be sent to this channel by any application at any time.
    /// Every message sent to this channel MUST be valid against one, and only one, of the message objects
    /// defined in this map.
    /// </remarks>
    public MessagesObject Messages { get; set; } = new();

    /// <summary>
    /// Gets or sets a human-friendly title for the channel.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Gets or sets a short summary of the channel.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Gets or sets an optional description of this channel.
    /// </summary>
    /// <remarks>
    /// An optional description of this channel. CommonMark syntax can be used for rich text representation.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets an array of $ref pointers to the definition of the servers in which this channel is available.
    /// </summary>
    /// <remarks>
    /// An array of `$ref` pointers to the definition of the servers in which this channel is available.
    /// If the channel is located in the root Channels Object, it MUST point to a subset of server definitions
    /// located in the root Servers Object, and MUST NOT point to a subset of server definitions located in
    /// the Components Object or anywhere else. If the channel is located in the Components Object, it MAY
    /// point to a Server Objects in any location. If `servers` is absent or empty, this channel MUST be available
    /// on all the servers defined in the Servers Object. Please note the `servers` property value MUST be an array
    /// of Reference Objects and, therefore, MUST NOT contain an array of Server Objects. However, it is RECOMMENDED
    /// that parsers (or other software) dereference this property for a better development experience.
    /// </remarks>
    public List<ReferenceObject> Servers { get; set; } = new();

    /// <summary>
    /// Gets or sets a map of the parameters included in the channel address.
    /// </summary>
    /// <remarks>
    /// A map of the parameters included in the channel address. It MUST be present only when the address contains
    /// Channel Address Expressions.
    /// </remarks>
    public ParametersObject Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets a list of tags for logical grouping of channels.
    /// </summary>
    public TagsObject Tags { get; set; } = new();

    /// <summary>
    /// Gets or sets additional external documentation for this channel.
    /// </summary>
    /// <remarks>
    /// Additional external documentation for this channel.
    /// </remarks>
    public ExternalDocumentationObject? ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets a map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the channel.
    /// </summary>
    /// <remarks>
    /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the channel.
    /// </remarks>
    public ChannelBindingsObject? Bindings { get; set; }
}
