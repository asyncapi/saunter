using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Pulsar
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/pulsar#server-binding-object
    /// </remarks>
    public class PulsarServerBinding
    {
        /// <summary>
        /// The pulsar tenant. If omitted, "public" must be assumed.
        /// </summary>
        [JsonProperty("tenant", NullValueHandling = NullValueHandling.Ignore)]
        public string Tenant { get; set; }
    }
}
