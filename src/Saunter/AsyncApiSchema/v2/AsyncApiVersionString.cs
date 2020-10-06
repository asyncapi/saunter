using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Saunter.Utils;

namespace Saunter.AsyncApiSchema.v2
{
    [JsonConverter(typeof(EnumMemberConverter))]
    public enum AsyncApiVersionString
    {
        [EnumMember(Value = "2.0.0")]
        v2,
    }
}