namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Allows referencing an external resource for extended documentation.
/// </summary>
public class ExternalDocumentationObject
{
    public ExternalDocumentationObject(string url)
    {
        Url = url;
    }

    /// <summary>
    /// Gets or sets a short description of the target documentation.
    /// </summary>
    /// <remarks>
    /// A short description of the target documentation.
    /// [CommonMark syntax](https://spec.commonmark.org/) can be used for rich text representation.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL for the target documentation.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** This MUST be in the form of an absolute URL.
    /// </remarks>
    public string Url { get; set; }
}

