using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// This object contains information about the message representation in HTTP.
/// </summary>
public class HttpMessageBinding
{
    public HttpMessageBinding(JsonSchema headers)
    {
        Headers = headers;
    }

    // A Schema object containing the definitions for HTTP-specific headers.
    // This schema MUST be of type 'object' and have a 'properties' key.
    public JsonSchema Headers { get; set; }

    // The version of this binding. If omitted, "latest" MUST be assumed.
    public string BindingVersion { get; set; } = "latest";
}
