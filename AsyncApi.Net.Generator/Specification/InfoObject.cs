namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Represents metadata about the API.
/// </summary>
public class InfoObject
{
    public InfoObject(string title, string version)
    {
        Title = title;
        Version = version;
    }

    /// <summary>
    /// Gets or sets the title of the application.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The title of the application.
    /// </remarks>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the version of the application API (not to be confused with the specification version).
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** Provides the version of the application API.
    /// </remarks>
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets a short description of the application.
    /// </summary>
    /// <remarks>
    /// A short description of the application. 
    /// [CommonMark syntax](https://spec.commonmark.org/) can be used for rich text representation.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a URL to the Terms of Service for the API. This MUST be in the form of an absolute URL.
    /// </summary>
    public string? TermsOfService { get; set; }

    /// <summary>
    /// Gets or sets the contact information for the exposed API.
    /// </summary>
    public ContactObject? Contact { get; set; }

    /// <summary>
    /// Gets or sets the license information for the exposed API.
    /// </summary>
    public LicenseObject? License { get; set; }

    /// <summary>
    /// Gets or sets a list of tags for application API documentation control.
    /// Tags can be used for logical grouping of applications.
    /// </summary>
    public TagsObject? Tags { get; set; }

    /// <summary>
    /// Gets or sets additional external documentation of the exposed API.
    /// </summary>
    public ExternalDocumentationObject? ExternalDocs { get; set; }
}

