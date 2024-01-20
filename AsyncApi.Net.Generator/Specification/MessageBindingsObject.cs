namespace AsyncApi.Net.Generator.Specification;

public class MessageBindingsObject
{
    // Protocol-specific information for an HTTP message, i.e., a request or a response.
    public HttpMessageBinding? Http { get; set; }

    // Protocol-specific information for a WebSockets message.
    public WebSocketsMessageBinding? Ws { get; set; }

    // Protocol-specific information for a Kafka message.
    public KafkaMessageBinding? Kafka { get; set; }

    // Protocol-specific information for a NATS message.
    public NATSMessageBinding? Nats { get; set; }

    // Protocol-specific information for a Redis message.
    public RedisMessageBinding? Redis { get; set; }
}
