using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Kafka
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/kafka#Operation-binding-object
    /// </remarks>
    public class KafkaOperationBinding
    {
        [JsonProperty("groupId")]
        public KafkaOperationBindingGroupId GroupId { get; set; }

        [JsonProperty("clientId")]
        public KafkaOperationBindingClientId ClientId { get; set; }

        [JsonProperty("bindingVersion")]
        public string BindingVersion { get; set; }
    }

    public class KafkaOperationBindingGroupId
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }
    }

    public class KafkaOperationBindingClientId
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }
    }
}
