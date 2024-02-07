using AsyncApiLibrary.Schema.v2.Bindings;
using AsyncApiLibrary.Schema.v2.Traits;

using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2;

public class Components : ICloneable
{
    /// <summary>
    /// An object to hold reusable Schema Objects.
    /// </summary>
    public Dictionary<string, JSchema> Schemas { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Objects.
    /// </summary>
    public Dictionary<string, Message> Messages { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Security Scheme Objects.
    /// </summary>
    public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Parameter Objects.
    /// </summary>
    public Dictionary<string, Parameter> Parameters { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Correlation ID Objects.
    /// </summary>
    public Dictionary<string, CorrelationId> CorrelationIds { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Server Binding Objects.
    /// </summary>
    public Dictionary<string, ServerBindings> ServerBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Channel Binding Objects.
    /// </summary>
    public Dictionary<string, ChannelBindings> ChannelBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Operation Binding Objects.
    /// </summary>
    public Dictionary<string, OperationBindings> OperationBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Binding Objects.
    /// </summary>
    public Dictionary<string, MessageBindings> MessageBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Operation Trait Objects.
    /// </summary>
    public Dictionary<string, OperationTrait> OperationTraits { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Trait Objects.
    /// </summary>
    public Dictionary<string, MessageTrait> MessageTraits { get; set; } = [];

    object ICloneable.Clone()
    {
        return Clone();
    }

    public Components Clone()
    {
        Components clone = new();

        clone.Schemas = Schemas.ToDictionary(p => p.Key, p => p.Value);
        clone.Messages = Messages.ToDictionary(p => p.Key, p => p.Value);
        clone.SecuritySchemes = SecuritySchemes.ToDictionary(p => p.Key, p => p.Value);
        clone.Parameters = Parameters.ToDictionary(p => p.Key, p => p.Value);
        clone.CorrelationIds = CorrelationIds.ToDictionary(p => p.Key, p => p.Value);
        clone.ServerBindings = ServerBindings.ToDictionary(p => p.Key, p => p.Value);
        clone.ChannelBindings = ChannelBindings.ToDictionary(p => p.Key, p => p.Value);
        clone.OperationBindings = OperationBindings.ToDictionary(p => p.Key, p => p.Value);
        clone.MessageBindings = MessageBindings.ToDictionary(p => p.Key, p => p.Value);
        clone.OperationTraits = OperationTraits.ToDictionary(p => p.Key, p => p.Value);
        clone.MessageTraits = MessageTraits.ToDictionary(p => p.Key, p => p.Value);

        return clone;
    }
}