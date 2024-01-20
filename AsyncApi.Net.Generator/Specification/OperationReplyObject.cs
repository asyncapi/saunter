using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes the reply part that MAY be applied to an <see cref="OperationObject"/>.
/// If an operation implements the request/reply pattern, the reply object represents the response message.
/// </summary>
public class OperationReplyObject
{
    /// <summary>
    /// Gets or sets the definition of the address that implementations MUST use for the reply.
    /// </summary>
    public OperationReplyAddressObject Address { get; set; }

    /// <summary>
    /// Gets or sets a <c>$ref</c> pointer to the definition of the channel in which this operation is performed.
    /// When <see cref="Address"/> is specified, the <c>address</c> property of the channel referenced by this property
    /// MUST be either <c>null</c> or not defined.
    /// If the operation reply is located inside a root <see cref="OperationObject"/>, it MUST point to a channel definition
    /// located in the root <see cref="ChannelsObject"/>, and MUST NOT point to a channel definition located in the
    /// <see cref="ComponentsObject"/> or anywhere else.
    /// If the operation reply is located inside an <see cref="OperationObject"/> in the <see cref="ComponentsObject"/>
    /// or in the <see cref="RepliesObject"/> in the <see cref="ComponentsObject"/>, it MAY point to a <see cref="ChannelObject"/>
    /// in any location.
    /// Please note the <c>channel</c> property value MUST be a <see cref="ReferenceObject"/> and, therefore,
    /// MUST NOT contain a <see cref="ChannelObject"/>.
    /// However, it is RECOMMENDED that parsers (or other software) dereference this property for a better development experience.
    /// </summary>
    public ReferenceObject Channel { get; set; }

    /// <summary>
    /// Gets or sets a list of <c>$ref</c> pointers pointing to the supported <see cref="MessageObject"/>s
    /// that can be processed by this operation as a reply.
    /// It MUST contain a subset of the messages defined in the channel referenced in this operation reply,
    /// and MUST NOT point to a subset of message definitions located in the <see cref="ComponentsObject"/>
    /// or anywhere else.
    /// Every message processed by this operation MUST be valid against one, and only one,
    /// of the message objects referenced in this list.
    /// Please note the <c>messages</c> property value MUST be a list of <see cref="ReferenceObject"/> and, therefore,
    /// MUST NOT contain <see cref="MessageObject"/>.
    /// However, it is RECOMMENDED that parsers (or other software) dereference this property for a better development experience.
    /// </summary>
    public List<ReferenceObject> Messages { get; set; }
}
