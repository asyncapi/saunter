using AsyncApiLibrary.Schema.v2.Bindings.Amqp;
using AsyncApiLibrary.Schema.v2.Bindings.Http;
using AsyncApiLibrary.Schema.v2.Bindings.Kafka;
using AsyncApiLibrary.Schema.v2.Bindings.Mqtt;

namespace AsyncApiLibrary.Schema.v2.Bindings;

/// <summary>
/// TODO: need to implement
/// </summary>
public class OperationBindings
{
    public HttpOperationBinding? Http { get; set; }

    public AmqpOperationBinding? Amqp { get; set; }

    public KafkaOperationBinding? Kafka { get; set; }

    public MqttOperationBinding? Mqtt { get; set; }
}