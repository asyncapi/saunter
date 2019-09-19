using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.Utils;

namespace Saunter.AsyncApiSchema.v2_0_0 {
    public class Components
    {
        [JsonProperty("schemas")]
        public IDictionary<string, OneOf<Schema, Reference>> Schemas { get; set; }

        [JsonProperty("messages")]
        public IDictionary<string, OneOf<Message, Reference>> Messages { get; set; }

        [JsonProperty("securitySchemes")]
        public IDictionary<string, OneOf<SecurityScheme, Reference>> SecuritySchemes { get; set; }

        [JsonProperty("parameters")]
        public IDictionary<string, OneOf<Parameter, Reference>> Parameters { get; set; }

        [JsonProperty("correlationIds")]
        public IDictionary<string, CorrelationId> CorrelationIds { get; set; }

        [JsonProperty("operationTraits")]
        public IDictionary<string, OperationTrait> OperationTraits { get; set; }

        [JsonProperty("messageTraits")]
        public IDictionary<string, MessageTrait> MessageTraits { get; set; }

        [JsonProperty("serverBindings")]
        public IDictionary<string, IServerBinding> ServerBindings { get; set; }

        [JsonProperty("channelBindings")]
        public IDictionary<string, IChannelBinding> ChannelBindings { get; set; }

        [JsonProperty("operationBindings")]
        public IDictionary<string, IOperationBinding> OperationBindings { get; set; }

        [JsonProperty("messageBindings")]
        public IDictionary<string, IMessageBinding> MessageBindings { get; set; }
        
        // todo: All the fixed fields declared above are objects that MUST use keys that match the regular expression: ^[a-zA-Z0-9\.\-_]+$.
    }
}