using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Http
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/http#Operation-Binding-Object
    /// </remarks>
    public class HttpOperationBinding : IOperationBinding
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("method")]
        public string? Method { get; set; }

        [JsonPropertyName("query")]
        public HttpOperationBindingQuery? Query { get; set; } 

        [JsonPropertyName("bindingVersion")]
        public string? BindingVersion { get; set; }
    }

    public class HttpOperationBindingQuery
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("required")]
        public string[]? Required { get; set; }

        [JsonPropertyName("properties")]
        public object? Properties { get; set; }

        [JsonPropertyName("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }
}
