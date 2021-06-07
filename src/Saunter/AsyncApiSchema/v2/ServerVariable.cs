using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2
{
    public class ServerVariable
    {
        /// <summary>
        /// An enumeration of string values to be used if the substitution options are from a limited set.
        /// </summary>
        [JsonProperty("enum")]
        public IList<string> Enum { get; set; }

        /// <summary>
        /// The default value to use for substitution, and to send, if an alternate value is not supplied.
        /// </summary>
        [JsonProperty("default")]
        public string Default { get; set; }

        /// <summary>
        /// An optional description for the server variable.
        /// CommonMark syntax MAY be used for rich text representation.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// An array of examples of the server variable.
        /// </summary>
        [JsonProperty("examples")]
        public IList<string> Examples { get; set; }
    }
}