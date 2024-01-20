using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Protocol-specific information for a WebSockets channel.
/// </summary>
public class WebSocketsChannelBinding
{
    /// <summary>
    /// The HTTP method to use when establishing the connection. Its value MUST be either `GET` or `POST`.
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// A Schema object containing the definitions for each query parameter.
    /// This schema MUST be of type `object` and have a `properties` key.
    /// </summary>
    public JsonSchema Query { get; set; }

    /// <summary>
    /// A Schema object containing the definitions of the HTTP headers to use when establishing the connection.
    /// This schema MUST be of type `object` and have a `properties` key.
    /// </summary>
    public JsonSchema Headers { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string BindingVersion { get; set; }
}
