using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#message-binding-object
    /// </remarks>
    public class AmqpMessageBinding : IMessageBinding
    {
        [JsonProperty("contentEncoding")]
        public string ContentEncoding { get; set; }

        [JsonProperty("messageType")]
        public string MessageType { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }
}