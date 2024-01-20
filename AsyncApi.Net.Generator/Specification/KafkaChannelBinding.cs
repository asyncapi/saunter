namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Protocol-specific information for a Kafka channel.
/// </summary>
public class KafkaChannelBinding
{
    /// <summary>
    /// Kafka topic name if different from channel name.
    /// </summary>
    public string? Topic { get; set; }

    /// <summary>
    /// Number of partitions configured on this topic (useful to know how many parallel consumers you may run).
    /// Must be positive.
    /// </summary>
    public int? Partitions { get; set; }

    /// <summary>
    /// Number of replicas configured on this topic.
    /// MUST be positive.
    /// </summary>
    public int? Replicas { get; set; }

    /// <summary>
    /// Topic configuration properties that are relevant for the API.
    /// </summary>
    public TopicConfiguration? TopicConfiguration { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string BindingVersion { get; set; } = "latest";
}
