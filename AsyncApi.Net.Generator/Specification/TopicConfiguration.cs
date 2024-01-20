using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Represents topic configuration in Kafka.
/// </summary>
public class TopicConfiguration
{
    /// <summary>
    /// The `cleanup.policy` configuration option.
    /// Array may only contain `delete` and/or `compact`.
    /// </summary>
    public List<string> CleanupPolicy { get; set; }

    /// <summary>
    /// The `retention.ms` configuration option.
    /// </summary>
    public int? RetentionMs { get; set; }

    /// <summary>
    /// The `retention.bytes` configuration option.
    /// </summary>
    public int? RetentionBytes { get; set; }

    /// <summary>
    /// The `delete.retention.ms` configuration option.
    /// </summary>
    public int? DeleteRetentionMs { get; set; }

    /// <summary>
    /// The `max.message.bytes` configuration option.
    /// </summary>
    public int? MaxMessageBytes { get; set; }
}
