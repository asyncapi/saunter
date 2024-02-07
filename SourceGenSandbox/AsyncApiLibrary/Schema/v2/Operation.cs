using AsyncApiLibrary.Schema.v2.Bindings;
using AsyncApiLibrary.Schema.v2.Traits;

namespace AsyncApiLibrary.Schema.v2;

/// <summary>
/// Describes a publish or a subscribe operation.
/// This provides a place to document how and why messages are sent and received. 
/// </summary>
public class Operation
{
    public Operation(string operationId, Message message)
    {
        OperationId = operationId;
        Message = message;
    }

    /// <summary>
    /// Unique string used to identify the operation.
    /// The id MUST be unique among all operations described in the API.
    /// The operationId value is case-sensitive.
    /// Tools and libraries MAY use the operationId to uniquely identify an operation,
    /// therefore, it is RECOMMENDED to follow common programming naming conventions.
    /// </summary>
    public string OperationId { get; set; }

    /// <summary>
    /// A short summary of what the operation is about.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the operation.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
    /// </summary>
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this operation.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol and the values describe
    /// protocol-specific definitions for the operation.
    /// </summary>
    public OperationBindings? Bindings { get; set; }

    /// <summary>
    /// A definition of the message that will be published or received on this channel.
    /// oneOf is allowed here to specify multiple messages, however, a message MUST be
    /// valid only against one of the referenced message objects.
    /// </summary>
    public Message Message { get; set; }

    /// <summary>
    /// A declaration of which security mechanisms are associated with this operation. 
    /// Only one of the security requirement objects MUST be satisfied to authorize an operation. 
    /// In cases where Server Security also applies, it MUST also be satisfied.
    /// </summary>
    public Security? Security { get; set; }

    /// <summary>
    /// A list of traits to apply to the operation object. Traits MUST be merged into the operation
    /// object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// </summary>
    public List<OperationTrait>? Traits { get; set; }
}

/// <summary>
/// Lists the required security schemes to execute this operation. 
/// The name used for each property MUST correspond to a security scheme 
/// declared in the Security Schemes under the Components Object.
/// TODO: implement
/// </summary>
public class Security : Dictionary<string, ICollection<string>> { }
