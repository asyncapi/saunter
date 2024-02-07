using AsyncApiLibrary.Schema.v2.Bindings;

namespace AsyncApiLibrary.Schema.v2.Traits;

public class OperationTrait
{
    public OperationTrait(string operationId)
    {
        OperationId = operationId;
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
    /// A free-form map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the operation.
    /// </summary>
    public OperationBindings? Bindings { get; set; }
}