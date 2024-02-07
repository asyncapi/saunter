namespace AsyncApiLibrary.Schema.v2;

public class Info
{
    public Info(string title)
    {
        Title = title;
    }

    /// <summary>
    /// The title of the application.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Provides the version of the application API
    /// (not to be confused with the specification version).
    /// </summary>
    public string Version { get; set; } = "latest";

    /// <summary>
    /// A short description of the application.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A URL to the Terms of Service for the API
    /// MUST be in the format of a URL.
    /// </summary>
    public string? TermsOfService { get; set; }

    /// <summary>
    /// The contact information for the exposed API.
    /// </summary>
    public Contact? Contact { get; set; }

    /// <summary>
    /// The license information for the exposed API.
    /// </summary>
    public License? License { get; set; }
}

public class Contact
{
    /// <summary>
    /// The identifying name of the contact person/organization.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The URL pointing to the contact information.
    /// MUST be in the format of a URL.
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// The email address of the contact person/organization.
    /// MUST be in the format of an email address.
    /// </summary>
    public string? Email { get; set; }
}

public class License
{
    public License(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The license name used for the API.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A URL to the license used for the API.
    /// MUST be in the format of a URL.
    /// </summary>
    public string? Url { get; set; }
}