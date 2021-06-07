using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Kafka
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/kafka#Message-binding-object
    /// </remarks>
    public class KafkaMessageBinding : IOperationBinding
    {
        [JsonProperty("key")]
        public KafkaOperationBindingGroupId Key { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class KafkaMessageBindingKey
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enum")]
        public string[] Enum { get; set; }
    }
}
