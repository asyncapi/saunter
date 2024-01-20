namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Map describing protocol-specific definitions for a server.
/// </summary>
public class ServerBindingsObject
{
    /// <summary>
    /// Gets or sets protocol-specific information for an HTTP server.
    /// </summary>
    public HttpServerBinding HTTP { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a WebSockets server.
    /// </summary>
    public WebSocketsServerBinding WS { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a Kafka server.
    /// </summary>
    public KafkaServerBinding Kafka { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a NATS server.
    /// </summary>
    public NATSServerBinding NATS { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific information for a Redis server.
    /// </summary>
    public RedisServerBinding Redis { get; set; }
}
