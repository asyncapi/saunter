using AsyncApiLibrary.Schema.v2.Bindings;

namespace AsyncApiLibrary.Schema.v2;

/// <summary>
/// Describes the operations available on a single channel.
/// </summary>
public class ChannelItem
{
    public ChannelItem(string channelId)
    {
        ChannelId = channelId;
    }

    /// <summary>
    /// Id channel, internal set as key to document map
    /// </summary>
    internal string ChannelId { get; set; }

    /// <summary>
    /// An optional description of this channel item.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The servers on which this channel is available, specified as an optional unordered
    /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
    /// If servers is absent or empty then this channel must be available on all servers
    /// defined in the Servers Object.
    /// </summary>
    public List<string>? Servers { get; set; }

    /// <summary>
    /// A definition of the SUBSCRIBE operation, which defines the messages produced by the application and sent to the channel.
    /// </summary>
    public Operation? Subscribe { get; set; }

    /// <summary>
    /// A definition of the PUBLISH operation, which defines the messages consumed by the application from the channel.
    /// </summary>
    public Operation? Publish { get; set; }

    /// <summary>
    /// A map of the parameters included in the channel name.
    /// It SHOULD be present only when using channels with expressions
    /// (as defined by RFC 6570 section 2.2).
    /// </summary>
    public Dictionary<string, Parameter>? Parameters { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol
    /// and the values describe protocol-specific definitions for the channel.
    /// </summary>
    public ChannelBindings? Bindings { get; set; }
}