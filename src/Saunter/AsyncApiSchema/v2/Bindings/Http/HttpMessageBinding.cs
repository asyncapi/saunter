using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Http
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/http#message-binding-object
    /// </remarks>
    public class HttpMessageBinding
    {
        [JsonProperty("headers")]
        public HttpMessageBindingHeaders Headers { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class HttpMessageBindingHeaders
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public object Properties { get; set; }
    }
}
