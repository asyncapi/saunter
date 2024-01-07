using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Amqp;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Http;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Kafka;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Mqtt;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

/// <summary>
/// MessageBindings can be either a the bindings or a reference to the bindings.
/// </summary>
public interface IMessageBindings { }

/// <summary>
/// A reference to the MessageBindings within the AsyncAPI components.
/// </summary>
public class MessageBindingsReference : Reference, IMessageBindings
{
    public MessageBindingsReference(string id) : base(id, "#/components/messageBindings/{0}") { }
}
/// <summary>
/// TODO: need to implement
/// </summary>
public class MessageBindings : IMessageBindings
{
    [JsonProperty("amqp", NullValueHandling = NullValueHandling.Ignore)]
    public AmqpMessageBinding? Amqp { get; set; }

    [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
    public HttpMessageBinding? Http { get; set; }

    [JsonProperty("kafka", NullValueHandling = NullValueHandling.Ignore)]
    public KafkaMessageBinding? Kafka { get; set; }

    [JsonProperty("mqtt", NullValueHandling = NullValueHandling.Ignore)]
    public MqttMessageBinding? Mqtt { get; set; }
}