using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#operation-binding-object
    /// </remarks>
    public class AmqpOperationBinding : IOperationBinding
    {
        [JsonPropertyName("expiration")]
        public int Expiration { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("cc")]
        public IList<string> Cc { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("deliveryMode")]
        public int DeliveryMode { get; set; }

        [JsonPropertyName("mandatory")]
        public bool? Mandatory { get; set; }

        [JsonPropertyName("bcc")]
        public IList<string> Bcc { get; set; }

        [JsonPropertyName("replyTo")]
        public string ReplyTo { get; set; }

        [JsonPropertyName("timestamp")]
        public bool? Timestamp { get; set; }

        [JsonPropertyName("ack")]
        public bool? Ack { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string BindingVersion { get; set; }
    }
}