using System;

using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

public class ExternalDocumentation
{
    /// <summary>
    /// A short description of the target documentation.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The URL for the target documentation.
    /// Value MUST be in the format of a URL.
    /// </summary>
    [JsonProperty("url")]
    public required string? Url { get; set; }
}