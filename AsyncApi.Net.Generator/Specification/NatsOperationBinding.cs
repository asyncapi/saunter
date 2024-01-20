namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Nats operation binding information.
/// </summary>
public class NatsOperationBinding
{
    /// <summary>
    /// Gets or sets the name of the queue to use. It MUST NOT exceed 255 characters.
    /// </summary>
    public string Queue { get; set; }

    /// <summary>
    /// Gets or sets the version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string BindingVersion { get; set; }
}
