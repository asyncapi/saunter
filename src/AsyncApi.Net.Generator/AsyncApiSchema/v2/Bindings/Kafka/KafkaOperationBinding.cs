using Newtonsoft.Json;

using NJsonSchema;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Kafka;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/kafka#Operation-binding-object
/// </remarks>
public class KafkaOperationBinding
{
    /// <summary>
    /// Id of the consumer group.
    /// </summary>
    [JsonProperty("groupId", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? GroupId { get; set; }

    /// <summary>
    /// Id of the consumer inside a consumer group.
    /// </summary>
    [JsonProperty("clientId", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? ClientId { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}
