using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2.Bindings.Pulsar
{
    /// <remarks>
    /// See: https://github.com/asyncapi/bindings/tree/master/pulsar#channel-binding-object
    /// </remarks>
    public class PulsarChannelBinding
    {
        /// <summary>
        /// The version of this binding. If omitted, "latest" MUST be assumed.
        /// </summary>
        [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
        public string BindingVersion { get; set; }

        /// <summary>
        /// The namespace associated with the topic.
        /// </summary>
        [JsonProperty("namespace", NullValueHandling = NullValueHandling.Ignore)]
        public string Namespace { get; set; }

        /// <summary>
        /// persistence of the topic in Pulsar persistent or non-persistent.
        /// </summary>
        [JsonProperty("persistence", NullValueHandling = NullValueHandling.Ignore)]
        public Persistence? Persistence { get; set; }

        /// <summary>
        /// Topic compaction threshold given in bytes.
        /// </summary>
        [JsonProperty("compaction", NullValueHandling = NullValueHandling.Ignore)]
        public int? Compaction { get; set; }

        /// <summary>
        /// A list of clusters the topic is replicated to.
        /// </summary>
        [JsonProperty("geoReplication", NullValueHandling = NullValueHandling.Ignore)]
        public IList<string> GeoReplication { get; set; }

        /// <summary>
        /// Topic retention policy.
        /// </summary>
        [JsonProperty("retention", NullValueHandling = NullValueHandling.Ignore)]
        public RetentionDefinition Retention { get; set; }

        /// <summary>
        /// Message Time-to-live in seconds.
        /// </summary>
        [JsonProperty("ttl", NullValueHandling = NullValueHandling.Ignore)]
        public int? TTL { get; set; }

        /// <summary>
        /// When Message deduplication is enabled, it ensures that each message produced on Pulsar topics is persisted to disk only once.
        /// </summary>
        [JsonProperty("deduplication", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Deduplication { get; set; }
    }

    public enum Persistence
    {
        [EnumMember(Value = "persistent")]
        Persistent,
        [EnumMember(Value = "non-persistent")]
        NonPersistent,
    }

    public class RetentionDefinition
    {
        /// <summary>
        /// Time given in Minutes. 0 = Disable message retention (by default).
        /// </summary>
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public int Time { get; set; }

        /// <summary>
        /// Size given in MegaBytes. 0 = Disable message retention (by default).
        /// </summary>
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public int Size { get; set; }
    }
}
