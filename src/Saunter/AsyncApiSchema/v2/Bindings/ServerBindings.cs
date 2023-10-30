using Newtonsoft.Json;

using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;
using Saunter.AsyncApiSchema.v2.Bindings.Mqtt;

namespace Saunter.AsyncApiSchema.v2.Bindings;

/// <summary>
/// ServerBindings can be either a the bindings or a reference to the bindings.
/// </summary>
public interface IServerBindings { }

/// <summary>
/// A reference to the OperationBindings within the AsyncAPI components.
/// </summary>
public class ServerBindingsReference : Reference, IServerBindings
{
    public ServerBindingsReference(string id) : base(id, "#/components/serverBindings/{0}") { }
}
/// <summary>
/// TODO: need to implement
/// </summary>
public class ServerBindings : IServerBindings
{
    [JsonProperty("amqp", NullValueHandling = NullValueHandling.Ignore)]
    public AmqpServerBinding? Amqp { get; set; }

    [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
    public HttpServerBinding? Http { get; set; }

    [JsonProperty("kafka", NullValueHandling = NullValueHandling.Ignore)]
    public KafkaServerBinding? Kafka { get; set; }

    [JsonProperty("mqtt", NullValueHandling = NullValueHandling.Ignore)]
    public MqttServerBinding? Mqtt { get; set; }
}