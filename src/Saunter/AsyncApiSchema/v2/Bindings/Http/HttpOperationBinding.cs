using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Http
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/http#Operation-Binding-Object
    /// </remarks>
    public class HttpOperationBinding : IOperationBinding
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; }

        [JsonPropertyName("query")]
        public object Query { get; set; } //need to check property type 

        [JsonPropertyName("bindingVersion")]
        public string BindingVersion { get; set; }
    }
}
