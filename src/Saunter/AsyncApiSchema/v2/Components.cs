using System;
using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.AsyncApiSchema.v2.Traits;

namespace Saunter.AsyncApiSchema.v2;

public class Components : ICloneable
{
    /// <summary>
    /// An object to hold reusable Schema Objects.
    /// </summary>
    [JsonProperty("schemas", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, JsonSchema> Schemas { get; set; } = new Dictionary<string, JsonSchema>();

    /// <summary>
    /// An object to hold reusable Message Objects.
    /// </summary>
    [JsonProperty("messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, Message> Messages { get; set; } = new Dictionary<string, Message>();

    /// <summary>
    /// An object to hold reusable Security Scheme Objects.
    /// </summary>
    [JsonProperty("securitySchemes", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<string, SecurityScheme>();

    /// <summary>
    /// An object to hold reusable Parameter Objects.
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, Parameter> Parameters { get; set; } = new Dictionary<string, Parameter>();

    /// <summary>
    /// An object to hold reusable Correlation ID Objects.
    /// </summary>
    [JsonProperty("correlationIds", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, CorrelationId> CorrelationIds { get; set; } = new Dictionary<string, CorrelationId>();

    /// <summary>
    /// An object to hold reusable Server Binding Objects.
    /// </summary>
    [JsonProperty("serverBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, ServerBindings> ServerBindings { get; set; } = new Dictionary<string, ServerBindings>();

    /// <summary>
    /// An object to hold reusable Channel Binding Objects.
    /// </summary>
    [JsonProperty("channelBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, ChannelBindings> ChannelBindings { get; set; } = new Dictionary<string, ChannelBindings>();

    /// <summary>
    /// An object to hold reusable Operation Binding Objects.
    /// </summary>
    [JsonProperty("operationBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, OperationBindings> OperationBindings { get; set; } = new Dictionary<string, OperationBindings>();

    /// <summary>
    /// An object to hold reusable Message Binding Objects.
    /// </summary>
    [JsonProperty("messageBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, MessageBindings> MessageBindings { get; set; } = new Dictionary<string, MessageBindings>();

    /// <summary>
    /// An object to hold reusable Operation Trait Objects.
    /// </summary>
    [JsonProperty("operationTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, OperationTrait> OperationTraits { get; set; } = new Dictionary<string, OperationTrait>();

    /// <summary>
    /// An object to hold reusable Message Trait Objects.
    /// </summary>
    [JsonProperty("messageTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, MessageTrait> MessageTraits { get; set; } = new Dictionary<string, MessageTrait>();

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