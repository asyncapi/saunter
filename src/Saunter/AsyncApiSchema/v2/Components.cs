using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2
{
    public class Components
    {
        [JsonPropertyName("schemas")]
        public IDictionary<ComponentFieldName, Schema> Schemas { get; set; } = new Dictionary<ComponentFieldName, Schema>();

        [JsonPropertyName("messages")]
        public IDictionary<ComponentFieldName, Message> Messages { get; set; } = new Dictionary<ComponentFieldName, Message>();

        [JsonPropertyName("securitySchemes")]
        public IDictionary<ComponentFieldName, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<ComponentFieldName, SecurityScheme>();

        [JsonPropertyName("parameters")]
        public IDictionary<ComponentFieldName, Parameter> Parameters { get; set; } = new Dictionary<ComponentFieldName, Parameter>();

        [JsonPropertyName("correlationIds")]
        public IDictionary<ComponentFieldName, CorrelationId> CorrelationIds { get; set; } = new Dictionary<ComponentFieldName, CorrelationId>();

        [JsonPropertyName("serverBindings")]
        public IDictionary<ComponentFieldName, IServerBinding> ServerBindings { get; set; } = new Dictionary<ComponentFieldName, IServerBinding>();

        [JsonPropertyName("channelBindings")]
        public IDictionary<ComponentFieldName, IChannelBinding> ChannelBindings { get; set; } = new Dictionary<ComponentFieldName, IChannelBinding>();

        [JsonPropertyName("operationBindings")]
        public IDictionary<ComponentFieldName, IOperationBinding> OperationBindings { get; set; } = new Dictionary<ComponentFieldName, IOperationBinding>();

        [JsonPropertyName("messageBindings")]
        public IDictionary<ComponentFieldName, IMessageBinding> MessageBindings { get; set; } = new Dictionary<ComponentFieldName, IMessageBinding>();

        [JsonPropertyName("operationTraits")]
        public IDictionary<ComponentFieldName, OperationTrait> OperationTraits { get; set; } = new Dictionary<ComponentFieldName, OperationTrait>();

        [JsonPropertyName("messageTraits")]
        public IDictionary<ComponentFieldName, MessageTrait> MessageTraits { get; set; } = new Dictionary<ComponentFieldName, MessageTrait>();
    }
}