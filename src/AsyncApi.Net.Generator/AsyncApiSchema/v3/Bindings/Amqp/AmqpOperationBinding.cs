using System.Collections.Generic;

using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Amqp;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#operation-binding-object
/// </remarks>
public class AmqpOperationBinding
{
    /// <summary>
    /// TTL (Time-To-Live) for the message. It MUST be greater than or equal to zero.
    /// </summary>
    [JsonProperty("expiration", NullValueHandling = NullValueHandling.Ignore)]
    public int Expiration { get; set; }

    /// <summary>
    /// Identifies the user who has sent the message.
    /// </summary>
    [JsonProperty("userId", NullValueHandling = NullValueHandling.Ignore)]
    public string? UserId { get; set; }

    /// <summary>
    /// The routing keys the message should be routed to at the time of publishing.
    /// </summary>
    [JsonProperty("cc", NullValueHandling = NullValueHandling.Ignore)]
    public IList<string>? Cc { get; set; }

    /// <summary>
    /// A priority for the message.
    /// </summary>
    [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
    public int Priority { get; set; }

    /// <summary>
    /// Delivery mode of the message. Its value MUST be either 1 (transient) or 2 (persistent).
    /// </summary>
    [JsonProperty("deliveryMode", NullValueHandling = NullValueHandling.Ignore)]
    public int DeliveryMode { get; set; }

    /// <summary>
    /// Whether the message is mandatory or not.
    /// </summary>
    [JsonProperty("mandatory", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Mandatory { get; set; }

    /// <summary>
    /// Like cc but consumers will not receive this information.
    /// </summary>
    [JsonProperty("bcc", NullValueHandling = NullValueHandling.Ignore)]
    public IList<string>? Bcc { get; set; }

    /// <summary>
    /// Name of the queue where the consumer should send the response.
    /// </summary>
    [JsonProperty("replyTo", NullValueHandling = NullValueHandling.Ignore)]
    public string? ReplyTo { get; set; }

    /// <summary>
    /// Whether the message should include a timestamp or not.
    /// </summary>
    [JsonProperty("timestamp", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Timestamp { get; set; }

    /// <summary>
    /// Whether the consumer should ack the message or not.
    /// </summary>
    [JsonProperty("ack", NullValueHandling = NullValueHandling.Ignore)]
    public bool? Ack { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}