using System.Collections.Generic;
using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings;

namespace Saunter.AsyncApiSchema.v2
{
    /// <summary>
    /// Describes the operations available on a single channel.
    /// </summary>
    public class ChannelItem
    {
        /// <summary>
        /// An optional description of this channel item.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// A definition of the SUBSCRIBE operation.
        /// </summary>
        [JsonProperty("subscribe")]
        public Operation Subscribe { get; set; }

        /// <summary>
        /// A definition of the PUBLISH operation.
        /// </summary>
        [JsonProperty("publish")]
        public Operation Publish { get; set; }

        /// <summary>
        /// A map of the parameters included in the channel name.
        /// It SHOULD be present only when using channels with expressions
        /// (as defined by RFC 6570 section 2.2).
        /// </summary>
        [JsonProperty("parameters")]
        public IList<IParameter> Parameters { get; set; } = new List<IParameter>();

        /// <summary>
        /// A free-form map where the keys describe the name of the protocol
        /// and the values describe protocol-specific definitions for the channel.
        /// </summary>
        [JsonProperty("bindings")]
        public ChannelBindings Bindings { get; set; }
    }
}