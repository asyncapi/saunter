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
    public string? AsyncApi { get; } = "3.0.0";

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
    public Dictionary<string, Server> Servers { get; set; } = [];

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
    public Dictionary<string, Channel> Channels { get; set; } = [];

    /// <summary>
    /// An element to hold various schemas for the specification.
    /// </summary>
    [JsonProperty("components")]
    public Components Components { get; set; } = new();

    [JsonIgnore]
    internal string? DocumentName { get; set; }

    public AsyncApiDocument Clone()
    {
        return new()
        {
            Id = Id,
            Info = Info,
            DefaultContentType = DefaultContentType,
            DocumentName = DocumentName,
            Channels = Channels.ToDictionary(c => c.Key, c => c.Value),
            Servers = Servers.ToDictionary(p => p.Key, p => p.Value),
            Components = Components.Clone()
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}