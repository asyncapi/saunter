using System.Collections.Generic;

using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Traits;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <summary>
/// Describes a publish or a subscribe operation.
/// This provides a place to document how and why messages are sent and received.
/// </summary>
public class Operation
{
    /// <summary>
    /// Unique string used to identify the operation.
    /// The id MUST be unique among all operations described in the API.
    /// The operationId value is case-sensitive.
    /// Tools and libraries MAY use the operationId to uniquely identify an operation,
    /// therefore, it is RECOMMENDED to follow common programming naming conventions.
    /// </summary>
    [JsonProperty("operationId", NullValueHandling = NullValueHandling.Ignore)]
    public required string OperationId { get; set; }

    /// <summary>
    /// A short summary of what the operation is about.
    /// </summary>
    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string? Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the operation.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
    /// </summary>
    [JsonProperty("tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this operation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol and the values describe
    /// protocol-specific definitions for the operation.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IOperationBindings? Bindings { get; set; }

    /// <summary>
    /// A definition of the message that will be published or received on this channel.
    /// oneOf is allowed here to specify multiple messages, however, a message MUST be
    /// valid only against one of the referenced message objects.
    /// </summary>
    [JsonProperty("message")]
    public required IMessage Message { get; set; }

    /// <summary>
    /// A declaration of which security mechanisms are associated with this operation.
    /// Only one of the security requirement objects MUST be satisfied to authorize an operation.
    /// In cases where Server Security also applies, it MUST also be satisfied.
    /// </summary>
    [JsonProperty("security", NullValueHandling = NullValueHandling.Ignore)]
    public Security? Security { get; set; }

    /// <summary>
    /// A list of traits to apply to the operation object. Traits MUST be merged into the operation
    /// object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// </summary>
    [JsonProperty("traits", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public List<IOperationTrait>? Traits { get; set; }
}

/// <summary>
/// Lists the required security schemes to execute this operation.
/// The name used for each property MUST correspond to a security scheme
/// declared in the Security Schemes under the Components Object.
/// TODO: implement
/// </summary>
public class Security : Dictionary<string, ICollection<string>> { }
