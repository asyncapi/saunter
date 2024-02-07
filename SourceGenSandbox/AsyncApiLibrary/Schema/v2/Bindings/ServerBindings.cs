using AsyncApiLibrary.Schema.v2.Bindings.Amqp;
using AsyncApiLibrary.Schema.v2.Bindings.Http;
using AsyncApiLibrary.Schema.v2.Bindings.Kafka;
using AsyncApiLibrary.Schema.v2.Bindings.Mqtt;

namespace AsyncApiLibrary.Schema.v2.Bindings;

/// <summary>
/// TODO: need to implement
/// </summary>
public class ServerBindings
{
    public AmqpServerBinding? Amqp { get; set; }

    public HttpServerBinding? Http { get; set; }

    public KafkaServerBinding? Kafka { get; set; }

    public MqttServerBinding? Mqtt { get; set; }
}