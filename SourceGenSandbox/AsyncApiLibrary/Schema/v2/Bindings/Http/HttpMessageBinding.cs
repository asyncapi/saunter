using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2.Bindings.Http;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/http#message-binding-object
/// </remarks>
public class HttpMessageBinding
{
    /// <summary>
    /// A Schema object containing the definitions for HTTP-specific headers.
    /// This schema MUST be of type object and have a properties key.
    /// </summary>
    public JSchema? Headers { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    public string? BindingVersion { get; set; }
}
