using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Http
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/http#Operation-Binding-Object
    /// </remarks>
    public class HttpOperationBinding
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("query")]
        public HttpOperationBindingQuery Query { get; set; } 

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class HttpOperationBindingQuery
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("required")]
        public string[] Required { get; set; }

        [JsonProperty("properties")]
        public object Properties { get; set; }

        [JsonProperty("additionalProperties")]
        public bool AdditionalProperties { get; set; }
    }
}
