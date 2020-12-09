using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#message-binding-object
    /// </remarks>
    public class AmqpMessageBinding : IMessageBinding
    {
        [JsonPropertyName("contentEncoding")]
        public string? ContentEncoding { get; set; }

        [JsonPropertyName("messageType")]
        public string? MessageType { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string? BindingVersion { get; set; }
    }
}