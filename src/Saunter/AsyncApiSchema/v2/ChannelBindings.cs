using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class ChannelBindings
    {
        [JsonPropertyName("amqp")]
        public AmqpChannelBinding? Amqp { get; set; }
    }
}