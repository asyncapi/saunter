using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Mqtt;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/mqtt/README.md#operation-binding-object
/// </remarks>
public class MqttOperationBinding
{
    /// <summary>
    /// Defines the Quality of Service (QoS) levels for the message flow between client and server.
    /// Its value MUST be either 0 (At most once delivery), 1 (At least once delivery), or 2 (Exactly once delivery).
    /// </summary>
    [JsonProperty("qos", NullValueHandling = NullValueHandling.Ignore)]
    public int? Qos { get; set; }

    /// <summary>
    /// Whether the broker should retain the message or not.
    /// </summary>
    [JsonProperty("retain", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Retain { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}