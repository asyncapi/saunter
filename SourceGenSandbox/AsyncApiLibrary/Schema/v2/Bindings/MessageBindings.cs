using AsyncApiLibrary.Schema.v2.Bindings.Amqp;
using AsyncApiLibrary.Schema.v2.Bindings.Http;
using AsyncApiLibrary.Schema.v2.Bindings.Kafka;
using AsyncApiLibrary.Schema.v2.Bindings.Mqtt;

namespace AsyncApiLibrary.Schema.v2.Bindings;

public class MessageBindings
{
    public AmqpMessageBinding? Amqp { get; set; }

    public HttpMessageBinding? Http { get; set; }

    public KafkaMessageBinding? Kafka { get; set; }

    public MqttMessageBinding? Mqtt { get; set; }
}