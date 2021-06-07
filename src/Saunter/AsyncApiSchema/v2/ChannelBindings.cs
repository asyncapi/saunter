using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;

namespace Saunter.AsyncApiSchema.v2
{
    public class ChannelBindings
    {
        [JsonProperty("amqp")]
        public AmqpChannelBinding Amqp { get; set; }
    }
}