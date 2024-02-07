namespace AsyncApiLibrary.Schema.v2.Bindings.Amqp;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/amqp/README.md#message-binding-object
/// </remarks>
public class AmqpMessageBinding
{
    /// <summary>
    /// A MIME encoding for the message content.
    /// </summary>
    public string? ContentEncoding { get; set; }

    /// <summary>
    /// Application-specific message type.
    /// </summary>
    public string? MessageType { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}