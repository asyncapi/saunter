namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Represents contact information for the exposed API.
/// </summary>
public class ContactObject
{
    public ContactObject(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the identifying name of the contact person/organization.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the URL pointing to the contact information.
    /// </summary>
    /// <remarks>
    /// This MUST be in the form of an absolute URL.
    /// </remarks>
    public string? Url { get; set; }

    /// <summary>
    /// Gets or sets the email address of the contact person/organization.
    /// </summary>
    /// <remarks>
    /// MUST be in the format of an email address.
    /// </remarks>
    public string? Email { get; set; }
}
