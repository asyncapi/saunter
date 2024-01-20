namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Allows configuration of the supported OAuth Flows.
/// </summary>
public class OAuthFlowsObject
{
    /// <summary>
    /// Gets or sets configuration for the OAuth Implicit flow.
    /// </summary>
    public OAuthFlowObject Implicit { get; set; }

    /// <summary>
    /// Gets or sets configuration for the OAuth Resource Owner Protected Credentials flow.
    /// </summary>
    public OAuthFlowObject Password { get; set; }

    /// <summary>
    /// Gets or sets configuration for the OAuth Client Credentials flow.
    /// </summary>
    public OAuthFlowObject ClientCredentials { get; set; }

    /// <summary>
    /// Gets or sets configuration for the OAuth Authorization Code flow.
    /// </summary>
    public OAuthFlowObject AuthorizationCode { get; set; }
}

