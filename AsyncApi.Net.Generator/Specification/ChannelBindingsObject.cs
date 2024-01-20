namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Map describing protocol-specific definitions for a channel.
/// </summary>
public class ChannelBindingsObject
{
    /// <summary>
    /// Protocol-specific information for an HTTP channel.
    /// </summary>
    public HttpChannelBinding? Http { get; set; }

    /// <summary>
    /// Protocol-specific information for a WebSockets channel.
    /// </summary>
    public WebSocketsChannelBinding? Ws { get; set; }

    /// <summary>
    /// Protocol-specific information for a Kafka channel.
    /// </summary>
    public KafkaChannelBinding? Kafka { get; set; }

    /// <summary>
    /// Protocol-specific information for a NATS channel.
    /// </summary>
    public NatsChannelBinding? Nats { get; set; }

    /// <summary>
    /// Protocol-specific information for a Redis channel.
    /// </summary>
    public RedisChannelBinding? Redis { get; set; }
}
