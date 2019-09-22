using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;

namespace Saunter.AsyncApiSchema.v2_0_0
{
    public class ChannelBindings
    {
        [JsonProperty("amqp")]
        public AmqpChannelBinding Amqp { get; set; }
    }
}