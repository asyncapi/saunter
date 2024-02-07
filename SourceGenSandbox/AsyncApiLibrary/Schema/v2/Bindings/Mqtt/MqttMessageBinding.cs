namespace AsyncApiLibrary.Schema.v2.Bindings.Mqtt;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/blob/master/mqtt/README.md#message-binding-object
/// </remarks>
public class MqttMessageBinding
{
    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}