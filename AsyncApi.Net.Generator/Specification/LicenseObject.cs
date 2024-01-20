namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Represents license information for the exposed API.
/// </summary>
public class LicenseObject
{
    public LicenseObject(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the license name used for the API.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The license name used for the API.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a URL to the license used for the API.
    /// </summary>
    /// <remarks>
    /// This MUST be in the form of an absolute URL.
    /// </remarks>
    public string? Url { get; set; }
}
