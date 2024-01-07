using System.Collections.Generic;

using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Traits;

/// <summary>
/// Can be either an <see cref="OperationTrait"/> or an <see cref="OperationTraitReference"/> reference to an operation trait.
/// </summary>
public interface IOperationTrait { }

/// <summary>
/// A reference to an OperationTrait within the AsyncAPI components.
/// </summary>
public class OperationTraitReference : Reference, IOperationTrait
{
    public OperationTraitReference(string id) : base(id, "#/components/operationTraits/{0}") { }
}

public class OperationTrait : IOperationTrait
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
    [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this operation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IOperationBindings? Bindings { get; set; }
}