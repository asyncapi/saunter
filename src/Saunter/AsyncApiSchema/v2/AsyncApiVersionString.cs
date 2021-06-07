using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Saunter.AsyncApiSchema.v2
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AsyncApiVersionString
    {
        [EnumMember(Value = "2.0.0")]
        v2,
    }
}