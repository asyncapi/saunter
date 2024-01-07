using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Amqp;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#message-binding-object
/// </remarks>
public class AmqpMessageBinding
{
    /// <summary>
    /// A MIME encoding for the message content.
    /// </summary>
    [JsonProperty("contentEncoding", NullValueHandling = NullValueHandling.Ignore)]
    public string? ContentEncoding { get; set; }

    /// <summary>
    /// Application-specific message type.
    /// </summary>
    [JsonProperty("messageType", NullValueHandling = NullValueHandling.Ignore)]
    public string? MessageType { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}