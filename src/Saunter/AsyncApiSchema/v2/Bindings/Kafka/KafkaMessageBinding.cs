using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Kafka
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/kafka#Message-binding-object
    /// </remarks>
    public class KafkaMessageBinding : IOperationBinding
    {
        [JsonPropertyName("key")]
        public KafkaOperationBindingGroupId Key { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class KafkaMessageBindingKey
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("enum")]
        public string[] Enum { get; set; }
    }
}
