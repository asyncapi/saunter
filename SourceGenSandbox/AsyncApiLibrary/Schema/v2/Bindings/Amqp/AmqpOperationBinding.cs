namespace AsyncApiLibrary.Schema.v2.Bindings.Amqp;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#operation-binding-object
/// </remarks>
public class AmqpOperationBinding
{
    /// <summary>
    /// TTL (Time-To-Live) for the message. It MUST be greater than or equal to zero.
    /// </summary>
    public int Expiration { get; set; }

    /// <summary>
    /// Identifies the user who has sent the message.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// The routing keys the message should be routed to at the time of publishing.
    /// </summary>
    public IList<string>? Cc { get; set; }

    /// <summary>
    /// A priority for the message.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Delivery mode of the message. Its value MUST be either 1 (transient) or 2 (persistent).
    /// </summary>
    public int DeliveryMode { get; set; }

    /// <summary>
    /// Whether the message is mandatory or not.
    /// </summary>
    public bool? Mandatory { get; set; }

    /// <summary>
    /// Like cc but consumers will not receive this information.
    /// </summary>
    public IList<string>? Bcc { get; set; }

    /// <summary>
    /// Name of the queue where the consumer should send the response.
    /// </summary>
    public string? ReplyTo { get; set; }

    /// <summary>
    /// Whether the message should include a timestamp or not.
    /// </summary>
    public bool? Timestamp { get; set; }

    /// <summary>
    /// Whether the consumer should ack the message or not.
    /// </summary>
    public bool? Ack { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}