using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2.Bindings.Kafka;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/kafka#Operation-binding-object
/// </remarks>
public class KafkaOperationBinding
{
    /// <summary>
    /// Id of the consumer group.
    /// </summary>
    public JSchema? GroupId { get; set; }

    /// <summary>
    /// Id of the consumer inside a consumer group.
    /// </summary>
    public JSchema? ClientId { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}
