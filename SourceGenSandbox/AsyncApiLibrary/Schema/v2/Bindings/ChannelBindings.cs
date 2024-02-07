using AsyncApiLibrary.Schema.v2.Bindings.Amqp;
using AsyncApiLibrary.Schema.v2.Bindings.Http;
using AsyncApiLibrary.Schema.v2.Bindings.Kafka;
using AsyncApiLibrary.Schema.v2.Bindings.Mqtt;

namespace AsyncApiLibrary.Schema.v2.Bindings;

/// <summary>
/// TODO: need to implement
/// </summary>
public class ChannelBindings
{
    public AmqpChannelBinding? Amqp { get; set; }

    public HttpChannelBinding? Http { get; set; }

    public KafkaChannelBinding? Kafka { get; set; }

    public MqttChannelBinding? Mqtt { get; set; }
}