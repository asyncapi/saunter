namespace AsyncApiLibrary.Schema.v2;

/// <summary>
/// Allows configuration of the supported OAuth Flows.
/// </summary>
public class OAuthFlows
{
    /// <summary>
    /// Configuration for the OAuth Implicit flow.
    /// </summary>
    public OAuthFlow? Implicit { get; set; }

    /// <summary>
    /// Configuration for the OAuth Resource Owner Protected Credentials flow.
    /// </summary>
    public OAuthFlow? Password { get; set; }

    /// <summary>
    /// Configuration for the OAuth Client Credentials flow.
    /// </summary>
    public OAuthFlow? ClientCredentials { get; set; }

    /// <summary>
    /// Configuration for the OAuth Authorization Code flow.
    /// </summary>
    public OAuthFlow? AuthorizationCode { get; set; }
}

/// <summary>
/// Configuration details for a supported OAuth Flow
/// </summary>
public class OAuthFlow
{
    public OAuthFlow(string authorizationUrl, string tokenUrl, Dictionary<string, string> scopes)
    {
        AuthorizationUrl = authorizationUrl;
        TokenUrl = tokenUrl;
        Scopes = scopes;
    }

    /// <summary>
    /// REQUIRED. The authorization URL to be used for this flow. 
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    public string AuthorizationUrl { get; set; }

    /// <summary>
    /// REQUIRED. The token URL to be used for this flow. 
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    public string TokenUrl { get; set; }

    /// <summary>
    /// The URL to be used for obtaining refresh tokens. 
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    public string? RefreshUrl { get; set; }

    /// <summary>
    /// REQUIRED. The available scopes for the OAuth2 security scheme.
    /// A map between the scope name and a short description for it.
    /// </summary>
    public Dictionary<string, string> Scopes { get; set; }
}