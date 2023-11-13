using Newtonsoft.Json;

using NJsonSchema;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Kafka;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/kafka#Message-binding-object
/// </remarks>
public class KafkaMessageBinding
{
    /// <summary>
    /// The message key.
    /// </summary>
    [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? Key { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}
