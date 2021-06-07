using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Saunter.AsyncApiSchema.v2
{
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
    }
}