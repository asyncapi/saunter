using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

public class SecurityScheme
{
    /// <summary>
    /// The type of the security scheme.
    /// </summary>
    [JsonProperty("type")]
    public required SecuritySchemeType Type { get; set; }

    /// <summary>
    /// A short description for security scheme.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The name of the header, query or cookie parameter to be used.
    /// </summary>
    [JsonProperty("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The location of the API key.
    /// Valid values are "user" and "password" for apiKey and "query", "header" or "cookie" for httpApiKey.
    /// </summary>
    [JsonProperty("in")]
    public required string In { get; set; }

    /// <summary>
    /// The name of the HTTP Authorization scheme to be used in the Authorization header as defined in RFC7235.
    /// </summary>
    [JsonProperty("scheme")]
    public required string Scheme { get; set; }

    /// <summary>
    /// A hint to the client to identify how the bearer token is formatted. Bearer tokens are usually generated
    /// by an author    ization server, so this information is primarily for documentation purposes.
    /// </summary>
    [JsonProperty("bearerFormat")]
    public string? BearerFormat { get; set; }

    /// <summary>
    /// An object containing configuration information for the flow types supported.
    /// </summary>
    [JsonProperty("flows")]
    public required OAuthFlows Flows { get; set; }

    /// <summary>
    /// OpenId Connect URL to discover OAuth2 configuration values. This MUST be in the form of a URL.
    /// </summary>
    [JsonProperty("openIdConnectUrl")]
    public required string OpenIdConnectUrl { get; set; }
}

[JsonConverter(typeof(StringEnumConverter))]
public enum SecuritySchemeType
{
    [EnumMember(Value = "userPassword")]
    UserPassword,

    [EnumMember(Value = "apiKey")]
    ApiKey,

    [EnumMember(Value = "X509")]
    X509,

    [EnumMember(Value = "symmetricEncryption")]
    SymmetricEncryption,

    [EnumMember(Value = "asymmetricEncryption")]
    AsymmetricEncryption,

    [EnumMember(Value = "httpApiKey")]
    HttpApiKey,

    [EnumMember(Value = "http")]
    Http,

    [EnumMember(Value = "oauth2")]
    OAuth2,

    [EnumMember(Value = "openIdConnect")]
    OpenIdConnect,

    [EnumMember(Value = "plain")]
    Plain,

    [EnumMember(Value = "scramSha256")]
    ScramSha256,

    [EnumMember(Value = "scramSha512")]
    ScramSha512,

    [EnumMember(Value = "gssapi")]
    GSSAPI,
}