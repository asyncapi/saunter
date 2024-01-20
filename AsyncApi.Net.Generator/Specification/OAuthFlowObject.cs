using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Configuration details for a supported OAuth Flow.
/// </summary>
public class OAuthFlowObject
{
    /// <summary>
    /// Gets or sets the authorization URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The authorization URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </remarks>
    public string AuthorizationUrl { get; set; }

    /// <summary>
    /// Gets or sets the token URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The token URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </remarks>
    public string TokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL to be used for obtaining refresh tokens.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    public string RefreshUrl { get; set; }

    /// <summary>
    /// Gets or sets the available scopes for the OAuth2 security scheme.
    /// A map between the scope name and a short description for it.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The available scopes for the OAuth2 security scheme.
    /// A map between the scope name and a short description for it.
    /// </remarks>
    public Dictionary<string, string> Scopes { get; set; }
}

