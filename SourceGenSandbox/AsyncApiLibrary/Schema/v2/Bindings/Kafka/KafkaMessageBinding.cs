using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2.Bindings.Kafka;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/kafka#Message-binding-object
/// </remarks>
public class KafkaMessageBinding
{
    /// <summary>
    /// The message key.
    /// </summary>
    public JSchema? Key { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}
