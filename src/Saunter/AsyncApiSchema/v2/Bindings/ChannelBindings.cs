using Newtonsoft.Json;

using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;
using Saunter.AsyncApiSchema.v2.Bindings.Mqtt;

namespace Saunter.AsyncApiSchema.v2.Bindings;

/// <summary>
/// ChannelBindings can be either a the bindings or a reference to the bindings.
/// </summary>
public interface IChannelBindings { }

/// <summary>
/// A reference to the ChannelBindings within the AsyncAPI components.
/// </summary>
public class ChannelBindingsReference : Reference, IChannelBindings
{
    public ChannelBindingsReference(string id) : base(id, "#/components/channelBindings/{0}") { }
}

public class ChannelBindings : IChannelBindings
{
    [JsonProperty("amqp", NullValueHandling = NullValueHandling.Ignore)]
    public AmqpChannelBinding Amqp { get; set; }

    [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
    public HttpChannelBinding Http { get; set; }

    [JsonProperty("kafka", NullValueHandling = NullValueHandling.Ignore)]
    public KafkaChannelBinding Kafka { get; set; }

    [JsonProperty("mqtt", NullValueHandling = NullValueHandling.Ignore)]
    public MqttChannelBinding Mqtt { get; set; }
}