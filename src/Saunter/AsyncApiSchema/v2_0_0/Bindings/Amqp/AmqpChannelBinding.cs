using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp
{
    public class AmqpChannelBinding : IChannelBinding
    {
        [JsonProperty("is")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AmqpChannelIs Is { get; set; }

        [JsonProperty("exchange")]
        public AmqpChannelExchange Exchange { get; set; }
    }

    public class AmqpChannelExchange
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AmqpExchangeType Type { get; set; }
    }

    public enum AmqpChannelIs
    {
        [EnumMember(Value = "routingKey")]
        RoutingKey,
    }

    public enum AmqpExchangeType
    {
        [EnumMember(Value = "topic")]
        Topic,
    }
}