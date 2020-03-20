using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Components
    {
        [JsonProperty("schemas")]
        public IDictionary<ComponentFieldName, Schema> Schemas { get; set; } = new Dictionary<ComponentFieldName, Schema>();

        [JsonProperty("messages")]
        public IDictionary<ComponentFieldName, Message> Messages { get; set; } = new Dictionary<ComponentFieldName, Message>();

        [JsonProperty("securitySchemes")]
        public IDictionary<ComponentFieldName, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<ComponentFieldName, SecurityScheme>();

        [JsonProperty("parameters")]
        public IDictionary<ComponentFieldName, Parameter> Parameters { get; set; } = new Dictionary<ComponentFieldName, Parameter>();

        [JsonProperty("correlationIds")]
        public IDictionary<ComponentFieldName, CorrelationId> CorrelationIds { get; set; } = new Dictionary<ComponentFieldName, CorrelationId>();

        [JsonProperty("serverBindings")]
        public IDictionary<ComponentFieldName, IServerBinding> ServerBindings { get; set; } = new Dictionary<ComponentFieldName, IServerBinding>();

        [JsonProperty("channelBindings")]
        public IDictionary<ComponentFieldName, IChannelBinding> ChannelBindings { get; set; } = new Dictionary<ComponentFieldName, IChannelBinding>();

        [JsonProperty("operationBindings")]
        public IDictionary<ComponentFieldName, IOperationBinding> OperationBindings { get; set; } = new Dictionary<ComponentFieldName, IOperationBinding>();

        [JsonProperty("messageBindings")]
        public IDictionary<ComponentFieldName, IMessageBinding> MessageBindings { get; set; } = new Dictionary<ComponentFieldName, IMessageBinding>();

        [JsonProperty("operationTraits")]
        public IDictionary<ComponentFieldName, OperationTrait> OperationTraits { get; set; } = new Dictionary<ComponentFieldName, OperationTrait>();

        [JsonProperty("messageTraits")]
        public IDictionary<ComponentFieldName, MessageTrait> MessageTraits { get; set; } = new Dictionary<ComponentFieldName, MessageTrait>();
    }
}