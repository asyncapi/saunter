using System.Collections.Generic;

using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <summary>
/// Describes the operations available on a single channel.
/// </summary>
public class Channel
{
    /// <summary>
    /// An optional string representation of this channel's address.
    /// The address is typically the "topic name", "routing key", "event type", or "path".
    /// When null or absent, it MUST be interpreted as unknown.
    /// This is useful when the address is generated dynamically at runtime or can't be known upfront.
    /// It MAY contain Channel Address Expressions.
    /// Query parameters and fragments SHALL NOT be used, instead use bindings to define them.
    /// </summary>
    [JsonProperty("address", NullValueHandling = NullValueHandling.Ignore)]
    public string? Address { get; set; }

    /// <summary>
    /// A map of the messages that will be sent to this channel by any application at any time.
    /// Every message sent to this channel MUST be valid against one, and only one, of the message objects defined in this map.
    /// </summary>
    [JsonProperty("address")]
    public Dictionary<string, IMessage> Messages { get; set; } = [];

    /// <summary>
    /// A human-friendly title for the channel.
    /// </summary>
    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string? Title { get; set; }

    /// <summary>
    /// A short summary of the channel.
    /// </summary>
    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string? Summary { get; set; }

    /// <summary>
    /// An optional description of this channel item.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// An array of $ref pointers to the definition of the servers in which this channel is available.
    /// If the channel is located in the root Channels Object,
    /// it MUST point to a subset of server definitions located in the root Servers Object,
    /// and MUST NOT point to a subset of server definitions located in the Components Object or anywhere else.
    /// If the channel is located in the Components Object, it MAY point to a Server Objects in any location.
    /// If servers is absent or empty, this channel MUST be available on all the servers defined in the Servers Object.
    /// Please note the servers property value MUST be an array of Reference Objects and, therefore,
    /// MUST NOT contain an array of Server Objects.
    /// However, it is RECOMMENDED that parsers (or other software) dereference this property for a better development experience.
    /// </summary>
    [JsonProperty("servers", NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? Servers { get; set; }

    /// <summary>
    /// A map of the parameters included in the channel address.
    /// It MUST be present only when the address contains Channel Address Expressions.
    /// </summary>
    [JsonProperty("parameters", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public Dictionary<string, IParameter>? Parameters { get; set; }

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    [JsonProperty("tags")]
    public HashSet<Tag> Tags { get; set; } = [];

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol
    /// and the values describe protocol-specific definitions for the channel.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IChannelBindings? Bindings { get; set; }
}