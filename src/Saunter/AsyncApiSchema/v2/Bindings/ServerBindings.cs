using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;

namespace Saunter.AsyncApiSchema.v2.Bindings
{
    // TODO: set-up references for all 4 binding types

    public class ServerBindings
    {
        [JsonProperty("amqp")]
        public AmqpServerBinding Amqp { get; set; }

        [JsonProperty("http")]
        public HttpServerBinding Http { get; set; }

        [JsonProperty("kafka")]
        public KafkaServerBinding Kafka { get; set; }
    }
}