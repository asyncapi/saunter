namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Protocol-specific information for a Kafka server.
/// </summary>
public class KafkaServerBinding
{
    /// <summary>
    /// Gets or sets the API URL for the Schema Registry used when producing Kafka messages (if a Schema Registry was used).
    /// </summary>
    /// <remarks>
    /// OPTIONAL. API URL for the Schema Registry used when producing Kafka messages (if a Schema Registry was used).
    /// </remarks>
    public string? SchemaRegistryUrl { get; set; }

    /// <summary>
    /// Gets or sets the vendor of Schema Registry and Kafka serdes library that should be used.
    /// Valid values are "apicurio", "confluent", "ibm", or "karapace".
    /// </summary>
    /// <remarks>
    /// OPTIONAL. The vendor of Schema Registry and Kafka serdes library that should be used
    /// (e.g., "apicurio", "confluent", "ibm", or "karapace").
    /// MUST NOT be specified if `SchemaRegistryUrl` is not specified.
    /// </remarks>
    public string? SchemaRegistryVendor { get; set; }

    /// <summary>
    /// Gets or sets the version of this binding.
    /// </summary>
    /// <remarks>
    /// OPTIONAL. The version of this binding.
    /// Default value is "latest".
    /// </remarks>
    public string BindingVersion { get; set; } = "latest";
}
