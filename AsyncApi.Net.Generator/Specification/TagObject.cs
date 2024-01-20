namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Allows adding meta data to a single tag.
/// </summary>
public class TagObject
{
    /// <summary>
    /// Gets or sets the name of the tag.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The name of the tag.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a short description for the tag.
    /// </summary>
    /// <remarks>
    /// A short description for the tag.
    /// [CommonMark syntax](https://spec.commonmark.org/) can be used for rich text representation.
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets additional external documentation for this tag.
    /// </summary>
    public ExternalDocumentationObject ExternalDocs { get; set; }
}

