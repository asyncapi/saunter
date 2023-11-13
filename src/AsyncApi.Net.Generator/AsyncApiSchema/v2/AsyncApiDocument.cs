using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using NJsonSchema.Converters;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

[JsonConverter(typeof(JsonReferenceConverter))]
public class AsyncApiDocument : ICloneable
{
    /// <summary>
    /// Specifies the AsyncAPI Specification version being used.
    /// </summary>
    [JsonProperty("asyncapi", NullValueHandling = NullValueHandling.Ignore)]
    public string? AsyncApi { get; } = "2.6.0";

    /// <summary>
    /// Identifier of the application the AsyncAPI document is defining.
    /// </summary>
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string? Id { get; set; }

    /// <summary>
    /// Provides metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    [JsonProperty("info", NullValueHandling = NullValueHandling.Ignore)]
    public required Info Info { get; set; }

    /// <summary>
    /// Provides connection details of servers.
    /// </summary>
    [JsonProperty("servers")]
    public Dictionary<string, Server> Servers { get; set; } = new();

    /// <summary>
    /// A string representing the default content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// </summary>
    [JsonProperty("defaultContentType", NullValueHandling = NullValueHandling.Ignore)]
    public string? DefaultContentType { get; set; } = "application/json";

    /// <summary>
    /// The available channels and messages for the API.
    /// </summary>
    [JsonProperty("channels")]
    public Dictionary<string, ChannelItem> Channels { get; set; } = new();

    /// <summary>
    /// An element to hold various schemas for the specification.
    /// </summary>
    [JsonProperty("components")]
    public Components Components { get; set; } = new();

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    [JsonProperty("tags")]
    public HashSet<Tag> Tags { get; set; } = new();

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    [JsonIgnore]
    internal string? DocumentName { get; set; }

    public AsyncApiDocument Clone()
    {
        AsyncApiDocument clone = new()
        {
            Id = Id,
            Info = Info,
            DefaultContentType = DefaultContentType,
            ExternalDocs = ExternalDocs,
            DocumentName = DocumentName,
            Channels = Channels.ToDictionary(c => c.Key, c => c.Value),
            Servers = Servers.ToDictionary(p => p.Key, p => p.Value),
            Components = Components.Clone()
        };

        if (Tags is not null)
        {
            clone.Tags = new();

            foreach (Tag tag in Tags)
            {
                clone.Tags.Add(tag);
            }
        }

        return clone;
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}