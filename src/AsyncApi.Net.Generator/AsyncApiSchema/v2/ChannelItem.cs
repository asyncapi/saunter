using System.Collections.Generic;

using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <summary>
/// Describes the operations available on a single channel.
/// </summary>
public class ChannelItem
{
    /// <summary>
    /// An optional description of this channel item.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// The servers on which this channel is available, specified as an optional unordered
    /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
    /// If servers is absent or empty then this channel must be available on all servers
    /// defined in the Servers Object.
    /// </summary>
    [JsonProperty("servers", NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? Servers { get; set; }

    /// <summary>
    /// A definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
    /// </summary>
    [JsonProperty("subscribe", NullValueHandling = NullValueHandling.Ignore)]
    public Operation? Subscribe { get; set; }

    /// <summary>
    /// A definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
    /// </summary>
    [JsonProperty("publish", NullValueHandling = NullValueHandling.Ignore)]
    public Operation? Publish { get; set; }

    /// <summary>
    /// A map of the parameters included in the channel name.
    /// It SHOULD be present only when using channels with expressions
    /// (as defined by RFC 6570 section 2.2).
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, IParameter>? Parameters { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol
    /// and the values describe protocol-specific definitions for the channel.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IChannelBindings? Bindings { get; set; }
}