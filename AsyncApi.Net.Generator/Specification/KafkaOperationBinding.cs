using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Kafka operation binding information.
/// </summary>
public class KafkaOperationBinding
{
    /// <summary>
    /// Gets or sets the Id of the consumer group.
    /// </summary>
    public JsonSchema? GroupId { get; set; }

    /// <summary>
    /// Gets or sets the Id of the consumer inside a consumer group.
    /// </summary>
    public JsonSchema? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string BindingVersion { get; set; } = "latest";
}
