using NJsonSchema;
using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;
using Saunter.AsyncApiSchema.v2.Traits;

namespace Saunter.AsyncApiSchema.v2
{
    public class Components
    {
        /// <summary>
        /// An object to hold reusable Schema Objects.
        /// </summary>
        [JsonProperty("schemas")]
        public IDictionary<ComponentFieldName, JsonSchema> Schemas { get; set; } = new Dictionary<ComponentFieldName, JsonSchema>();

        /// <summary>
        /// An object to hold reusable Message Objects.
        /// </summary>
        [JsonProperty("messages")]
        public IDictionary<ComponentFieldName, Message> Messages { get; set; } = new Dictionary<ComponentFieldName, Message>();

        /// <summary>
        /// An object to hold reusable Security Scheme Objects.
        /// </summary>
        [JsonProperty("securitySchemes")]
        public IDictionary<ComponentFieldName, SecurityScheme> SecuritySchemes { get; set; } = new Dictionary<ComponentFieldName, SecurityScheme>();

        /// <summary>
        /// An object to hold reusable Parameter Objects.
        /// </summary>
        [JsonProperty("parameters")]
        public IDictionary<ComponentFieldName, Parameter> Parameters { get; set; } = new Dictionary<ComponentFieldName, Parameter>();

        /// <summary>
        /// An object to hold reusable Correlation ID Objects.
        /// </summary>
        [JsonProperty("correlationIds")]
        public IDictionary<ComponentFieldName, CorrelationId> CorrelationIds { get; set; } = new Dictionary<ComponentFieldName, CorrelationId>();
        
        /// <summary>
        /// An object to hold reusable Server Binding Objects.
        /// </summary>
        [JsonProperty("serverBindings")]
        public IDictionary<ComponentFieldName, ServerBindings> ServerBindings { get; set; } = new Dictionary<ComponentFieldName, ServerBindings>();

        /// <summary>
        /// An object to hold reusable Channel Binding Objects.
        /// </summary>
        [JsonProperty("channelBindings")]
        public IDictionary<ComponentFieldName, ChannelBindings> ChannelBindings { get; set; } = new Dictionary<ComponentFieldName, ChannelBindings>();

        /// <summary>
        /// An object to hold reusable Operation Binding Objects.
        /// </summary>
        [JsonProperty("operationBindings")]
        public IDictionary<ComponentFieldName, OperationBindings> OperationBindings { get; set; } = new Dictionary<ComponentFieldName, OperationBindings>();

        /// <summary>
        /// An object to hold reusable Message Binding Objects.
        /// </summary>
        [JsonProperty("messageBindings")]
        public IDictionary<ComponentFieldName, MessageBindings> MessageBindings { get; set; } = new Dictionary<ComponentFieldName, MessageBindings>();

        
        /// <summary>
        /// An object to hold reusable Operation Trait Objects.
        /// </summary>
        [JsonProperty("operationTraits")]
        public IDictionary<ComponentFieldName, OperationTrait> OperationTraits { get; set; } = new Dictionary<ComponentFieldName, OperationTrait>();

        /// <summary>
        /// An object to hold reusable Message Trait Objects.
        /// </summary>
        [JsonProperty("messageTraits")]
        public IDictionary<ComponentFieldName, MessageTrait> MessageTraits { get; set; } = new Dictionary<ComponentFieldName, MessageTrait>();
    }
}