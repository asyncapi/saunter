using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;

namespace Saunter.AsyncApiSchema.v2.Bindings
{
    // TODO: set-up references for all 4 binding types

    public class OperationBindings
    {         
        [JsonProperty("http")]
        public HttpOperationBinding Http { get; set; }

        [JsonProperty("amqp")]
        public AmqpOperationBinding Amqp { get; set; }

        [JsonProperty("kafka")]
        public KafkaOperationBinding Kafka { get; set; }
    }
}