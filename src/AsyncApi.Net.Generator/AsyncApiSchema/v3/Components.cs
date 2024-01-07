using System;
using NJsonSchema;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Traits;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

public class Components : ICloneable
{
    /// <summary>
    /// An object to hold reusable Schema Objects.
    /// </summary>
    [JsonProperty("schemas", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, JsonSchema> Schemas { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Objects.
    /// </summary>
    [JsonProperty("messages", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, Message> Messages { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Security Scheme Objects.
    /// </summary>
    [JsonProperty("securitySchemes", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, SecurityScheme> SecuritySchemes { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Parameter Objects.
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, Parameter> Parameters { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Correlation ID Objects.
    /// </summary>
    [JsonProperty("correlationIds", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, CorrelationId> CorrelationIds { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Server Binding Objects.
    /// </summary>
    [JsonProperty("serverBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, ServerBindings> ServerBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Channel Binding Objects.
    /// </summary>
    [JsonProperty("channelBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, ChannelBindings> ChannelBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Operation Binding Objects.
    /// </summary>
    [JsonProperty("operationBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, OperationBindings> OperationBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Binding Objects.
    /// </summary>
    [JsonProperty("messageBindings", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, MessageBindings> MessageBindings { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Operation Trait Objects.
    /// </summary>
    [JsonProperty("operationTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, OperationTrait> OperationTraits { get; set; } = [];

    /// <summary>
    /// An object to hold reusable Message Trait Objects.
    /// </summary>
    [JsonProperty("messageTraits", DefaultValueHandling = DefaultValueHandling.Ignore)]
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