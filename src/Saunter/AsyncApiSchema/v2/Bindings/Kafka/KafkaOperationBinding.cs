using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2.Bindings.Kafka
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/kafka#Operation-binding-object
    /// </remarks>
    public class KafkaOperationBinding : IOperationBinding
    {
        [JsonPropertyName("groupId")]
        public KafkaOperationBindingGroupId? GroupId { get; set; }

        [JsonPropertyName("clientId")]
        public KafkaOperationBindingClientId? ClientId { get; set; }

        [JsonPropertyName("bindingVersion")]
        public string? BindingVersion { get; set; }
    }

    public class KafkaOperationBindingGroupId
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("enum")]
        public string[]? Enum { get; set; }
    }

    public class KafkaOperationBindingClientId
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("enum")]
        public string[]? Enum { get; set; }
    }
}
