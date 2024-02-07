namespace AsyncApiLibrary.Schema.v2;

public readonly struct ServerProtocol
{
    public const string Amqp = "amqp";

    public const string Amqps = "amqps";

    public const string Nats = "nats";

    public const string NatsJetSteam = "nats-js";

    public const string Http = "http";

    public const string Https = "https";

    public const string Jms = "jms";

    public const string Kafka = "kafka";

    public const string KafkaSecure = "kafka-secure";

    public const string Mqtt = "mqtt";

    public const string SecureMqtt = "secure-mqtt";

    public const string Stomp = "stomp";

    public const string Stomps = "stomps";

    public const string Ws = "ws";

    public const string Wss = "wss";
}