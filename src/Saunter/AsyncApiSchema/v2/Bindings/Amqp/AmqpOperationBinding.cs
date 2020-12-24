using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#operation-binding-object
    /// </remarks>
    public class AmqpOperationBinding
    {
        [JsonProperty("expiration")]
        public int Expiration { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("cc")]
        public IList<string> Cc { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("deliveryMode")]
        public int DeliveryMode { get; set; }

        [JsonProperty("mandatory")]
        public bool? Mandatory { get; set; }

        [JsonProperty("bcc")]
        public IList<string> Bcc { get; set; }

        [JsonProperty("replyTo")]
        public string ReplyTo { get; set; }

        [JsonProperty("timestamp")]
        public bool? Timestamp { get; set; }

        [JsonProperty("ack")]
        public bool? Ack { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }
}