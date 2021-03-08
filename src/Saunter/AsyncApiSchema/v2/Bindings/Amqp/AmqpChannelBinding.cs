using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Amqp
{
    /// <remarks>
    /// https://github.com/asyncapi/bindings/blob/master/amqp/README.md#channel
    /// </remarks>
    public class AmqpChannelBinding : IChannelBinding
    {
        [JsonPropertyName("is")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AmqpChannelBindingIs Is { get; set; }

        [JsonPropertyName("exchange")]
        public AmqpChannelBindingExchange Exchange { get; set; }

        [JsonPropertyName("queue")]
        public AmqpChannelBindingQueue Queue { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class AmqpChannelBindingExchange
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("durable")]
        public bool? Durable { get; set; }

        [JsonPropertyName("autoDelete")]
        public bool? AutoDelete { get; set; }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
    }

    public class AmqpChannelBindingQueue
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("durable")]
        public bool? Durable { get; set; }

        [JsonPropertyName("exclusive")]
        public bool? Exclusive { get; set; }

        [JsonPropertyName("autoDelete")]
        public bool? AutoDelete { get; set; }

        [JsonPropertyName("vhost")]
        public string VirtualHost { get; set; }
    }

    public enum AmqpChannelBindingIs
    {
        [EnumMember(Value = "routingKey")]
        RoutingKey,

        [EnumMember(Value = "queue")]
        Queue,
    }

    public static class AmqpChannelBindingExchangeType
    {
        public const string Topic = "topic";

        public const string Direct = "direct";

        public const string Fanout = "fanout";

        public const string Default = "default";

        public const string Headers = "headers";
    }
}