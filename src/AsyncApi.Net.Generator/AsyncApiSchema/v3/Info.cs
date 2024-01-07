using Newtonsoft.Json;

using System.Collections.Generic;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;
public class Info
{
    /// <summary>
    /// The title of the application.
    /// </summary>
    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public required string Title { get; set; }

    /// <summary>
    /// Provides the version of the application API
    /// (not to be confused with the specification version).
    /// </summary>
    [JsonProperty("version", NullValueHandling = NullValueHandling.Ignore)]
    public string Version { get; set; } = "latest";

    /// <summary>
    /// A short description of the application.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// A URL to the Terms of Service for the API
    /// MUST be in the format of a URL.
    /// </summary>
    [JsonProperty("termsOfService", NullValueHandling = NullValueHandling.Ignore)]
    public string? TermsOfService { get; set; }

    /// <summary>
    /// The contact information for the exposed API.
    /// </summary>
    [JsonProperty("contact", NullValueHandling = NullValueHandling.Ignore)]
    public Contact? Contact { get; set; }

    /// <summary>
    /// The license information for the exposed API.
    /// </summary>
    [JsonProperty("license", NullValueHandling = NullValueHandling.Ignore)]
    public License? License { get; set; }

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    [JsonProperty("tags")]
    public HashSet<Tag> Tags { get; set; } = [];

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }
}

public class Contact
{
    /// <summary>
    /// The identifying name of the contact person/organization.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The URL pointing to the contact information.
    /// MUST be in the format of a URL.
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }

    /// <summary>
    /// The email address of the contact person/organization.
    /// MUST be in the format of an email address.
    /// </summary>
    [JsonProperty("email")]
    public string? Email { get; set; }
}

public class License
{
    /// <summary>
    /// The license name used for the API.
    /// </summary>
    [JsonProperty("name")]
    public required string Name { get; set; }

    /// <summary>
    /// A URL to the license used for the API.
    /// MUST be in the format of a URL.
    /// </summary>
    [JsonProperty("url")]
    public string? Url { get; set; }
}