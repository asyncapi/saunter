using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Http
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/http#message-binding-object
    /// </remarks>
    public class HttpMessageBinding : IMessageBinding
    {
        [JsonPropertyName("headers")]
        public string Headers { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string BindingVersion { get; set; }
    }
}
