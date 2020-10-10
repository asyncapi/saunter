using System.Text.Json;
using System.Text.Json.Serialization;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;

namespace Saunter.AsyncApiSchema.v2
{
    public class ChannelBindings
    {
        [JsonPropertyName("amqp")]
        public AmqpChannelBinding Amqp { get; set; }
    }
}