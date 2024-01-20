namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Map describing protocol-specific definitions for an operation.
/// </summary>
public class OperationBindingsObject
{
    /// <summary>
    /// Gets or sets protocol-specific information for an HTTP operation.
    /// </summary>
    public HttpOperationBinding Http { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a WebSockets operation.
    /// </summary>
    public WebSocketsOperationBinding Ws { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a Kafka operation.
    /// </summary>
    public KafkaOperationBinding Kafka { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a NATS operation.
    /// </summary>
    public NatsOperationBinding Nats { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a Redis operation.
    /// </summary>
    public RedisOperationBinding Redis { get; set; }
}
