using System.Collections.Generic;

using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <summary>
/// Allows configuration of the supported OAuth Flows.
/// </summary>
public class OAuthFlows
{
    /// <summary>
    /// Configuration for the OAuth Implicit flow.
    /// </summary>
    [JsonProperty("implicit")]
    public OAuthFlow? Implicit { get; set; }

    /// <summary>
    /// Configuration for the OAuth Resource Owner Protected Credentials flow.
    /// </summary>
    [JsonProperty("password")]
    public OAuthFlow? Password { get; set; }

    /// <summary>
    /// Configuration for the OAuth Client Credentials flow.
    /// </summary>
    [JsonProperty("clientCredentials")]
    public OAuthFlow? ClientCredentials { get; set; }

    /// <summary>
    /// Configuration for the OAuth Authorization Code flow.
    /// </summary>
    [JsonProperty("authorizationCode")]
    public OAuthFlow? AuthorizationCode { get; set; }
}

/// <summary>
/// Configuration details for a supported OAuth Flow
/// </summary>
public class OAuthFlow
{
    /// <summary>
    /// REQUIRED. The authorization URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    [JsonProperty("authorizationUrl")]
    public required string AuthorizationUrl { get; set; }

    /// <summary>
    /// REQUIRED. The token URL to be used for this flow.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    [JsonProperty("tokenUrl")]
    public required string TokenUrl { get; set; }

    /// <summary>
    /// The URL to be used for obtaining refresh tokens.
    /// This MUST be in the form of an absolute URL.
    /// </summary>
    [JsonProperty("refreshUrl")]
    public string? RefreshUrl { get; set; }

    /// <summary>
    /// REQUIRED. The available scopes for the OAuth2 security scheme.
    /// A map between the scope name and a short description for it.
    /// </summary>
    [JsonProperty("scopes")]
    public required Dictionary<string, string> Scopes { get; set; }
}