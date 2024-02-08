namespace AsyncApiLibrary.Schema.v2;

public interface IAsyncApiDocument
{
    /// <summary>
    /// Specifies the AsyncAPI Specification version being used.
    /// </summary>
    string? AsyncApi { get; }

    /// <summary>
    /// Identifier of the application the AsyncAPI document is defining.
    /// </summary>
    string? Id { get; set; }

    /// <summary>
    /// Provides metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    Info Info { get; set; }

    /// <summary>
    /// Provides connection details of servers.
    /// </summary>
    Dictionary<string, Server> Servers { get; set; }

    /// <summary>
    /// A string representing the default content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// </summary>
    string? DefaultContentType { get; set; }

    /// <summary>
    /// The available channels and messages for the API.
    /// </summary>
    Dictionary<string, ChannelItem> Channels { get; }

    /// <summary>
    /// An element to hold various schemas for the specification.
    /// </summary>
    Components Components { get; set; }

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    HashSet<Tag> Tags { get; set; }

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    ExternalDocumentation? ExternalDocs { get; set; }
}
