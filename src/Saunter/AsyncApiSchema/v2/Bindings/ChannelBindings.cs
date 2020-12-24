using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;

namespace Saunter.AsyncApiSchema.v2.Bindings
{
    // TODO: set-up references for all 4 binding types
    public interface IChannelBinding { }

    public class ChannelBindings
    {
        [JsonProperty("amqp")]
        public AmqpChannelBinding Amqp { get; set; }

        [JsonProperty("http")]
        public HttpChannelBinding Http { get; set; }

        [JsonProperty("kafka")]
        public KafkaChannelBinding Kafka { get; set; }
    }
}