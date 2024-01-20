using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a trait that MAY be applied to an <see cref="OperationObject"/>.
/// This object MAY contain any property from the <see cref="OperationObject"/>,
/// except the <c>action</c>, <c>channel</c>, and <c>traits</c> ones.
/// </summary>
public class OperationTraitObject
{
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
}
