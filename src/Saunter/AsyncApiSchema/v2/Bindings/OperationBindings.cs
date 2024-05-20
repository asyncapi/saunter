using Newtonsoft.Json;

using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;
using Saunter.AsyncApiSchema.v2.Bindings.Mqtt;

namespace Saunter.AsyncApiSchema.v2.Bindings;

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

public class OperationBindings : IOperationBindings
{
    [JsonProperty("http", NullValueHandling = NullValueHandling.Ignore)]
    public HttpOperationBinding Http { get; set; }

    [JsonProperty("amqp", NullValueHandling = NullValueHandling.Ignore)]
    public AmqpOperationBinding Amqp { get; set; }

    [JsonProperty("kafka", NullValueHandling = NullValueHandling.Ignore)]
    public KafkaOperationBinding Kafka { get; set; }

    [JsonProperty("mqtt", NullValueHandling = NullValueHandling.Ignore)]
    public MqttOperationBinding Mqtt { get; set; }
}
