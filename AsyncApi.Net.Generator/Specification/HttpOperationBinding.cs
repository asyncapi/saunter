using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// HTTP operation binding information.
/// </summary>
public class HttpOperationBinding
{
    public HttpOperationBinding(string method, JsonSchema query)
    {
        Method = method;
        Query = query;
    }

    /// <summary>
    /// Gets or sets the HTTP method for the request. Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
    /// </summary>
    public string Method { get; set; }

    /// <summary>
    /// Gets or sets a Schema object containing the definitions for each query parameter. This schema MUST be of type object and have a properties key.
    /// </summary>
    public JsonSchema Query { get; set; }

    /// <summary>
    /// Gets or sets the version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string BindingVersion { get; set; } = "latest";
}
