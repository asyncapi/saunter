using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class Server
    {
        public Server(string url, string protocol)
        {
            Url = url;
            Protocol = protocol;
        }

        [JsonPropertyName("url")]
        public string Url { get; }

        [JsonPropertyName("protocol")]
        public string Protocol { get; }

        [JsonPropertyName("protocolVersion")]
        public string ProtocolVersion { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("variables")]
        public IDictionary<string, ServerVariable> Variables { get; set; }

        [JsonPropertyName("security")]
        public IList<SecurityRequirement> Security { get; set; }

        [JsonPropertyName("bindings")]
        public ServerBindings Bindings { get; set; }
    }

    public static class ServerProtocol
    {
        public const string Amqp = "amqp";

        public const string Amqps = "amqps";

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
}