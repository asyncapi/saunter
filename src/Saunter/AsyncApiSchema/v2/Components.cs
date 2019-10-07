using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class Components
    {
        [JsonProperty("schemas")]
        public IDictionary<ComponentFieldName, NJsonSchema.JsonSchema> Schemas { get; set; }

        [JsonProperty("messages")]
        public IDictionary<ComponentFieldName, Message> Messages { get; set; }

        [JsonProperty("securitySchemes")]
        public IDictionary<ComponentFieldName, SecurityScheme> SecuritySchemes { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<ComponentFieldName, Parameter> Parameters { get; set; }

        [JsonProperty("correlationIds")]
        public IDictionary<ComponentFieldName, CorrelationId> CorrelationIds { get; set; }

        [JsonProperty("operationTraits")]
        public IDictionary<ComponentFieldName, OperationTrait> OperationTraits { get; set; }

        [JsonProperty("messageTraits")]
        public IDictionary<ComponentFieldName, MessageTrait> MessageTraits { get; set; }

        [JsonProperty("serverBindings")]
        public IDictionary<ComponentFieldName, IServerBinding> ServerBindings { get; set; }

        [JsonProperty("channelBindings")]
        public IDictionary<ComponentFieldName, IChannelBinding> ChannelBindings { get; set; }

        [JsonProperty("operationBindings")]
        public IDictionary<ComponentFieldName, IOperationBinding> OperationBindings { get; set; }

        [JsonProperty("messageBindings")]
        public IDictionary<ComponentFieldName, IMessageBinding> MessageBindings { get; set; }
    }
}