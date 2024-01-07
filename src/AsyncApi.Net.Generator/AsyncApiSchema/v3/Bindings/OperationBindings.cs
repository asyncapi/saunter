using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Amqp;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Http;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Kafka;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Mqtt;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

/// <summary>
/// OperationBindings can be either a the bindings or a reference to the bindings.
/// </summary>
public interface IOperationBindings { }

/// <summary>
/// A reference to the OperationBindings within the AsyncAPI components.
/// </summary>
public class OperationBindingsReference : Reference, IOperationBindings
{
    public OperationBindingsReference(string id) : base(id, "#/components/operationBindings/{0}") { }
}
/// <summary>
/// TODO: need to implement
/// </summary>
public class OperationBindings : IOperationBindings
{
    [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
    public HttpOperationBinding? Http { get; set; }

    [JsonProperty("amqp", NullValueHandling = NullValueHandling.Ignore)]
    public AmqpOperationBinding? Amqp { get; set; }

    [JsonProperty("kafka", NullValueHandling = NullValueHandling.Ignore)]
    public KafkaOperationBinding? Kafka { get; set; }

    [JsonProperty("mqtt", NullValueHandling = NullValueHandling.Ignore)]
    public MqttOperationBinding? Mqtt { get; set; }
}