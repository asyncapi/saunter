using System.Runtime.Serialization;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using NJsonSchema;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings.Http;

/// <remarks>
/// See: https://github.com/asyncapi/bindings/tree/master/http#Operation-Binding-Object
/// </remarks>
public class HttpOperationBinding
{
    /// <summary>
    /// Required. Type of operation. Its value MUST be either request or response.
    /// </summary>
    [JsonProperty("type")]
    public HttpOperationBindingType? Type { get; set; }

    /// <summary>
    /// When type is request, this is the HTTP method, otherwise it MUST be ignored.
    /// Its value MUST be one of GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS, CONNECT, and TRACE.
    /// </summary>
    [JsonProperty("method", NullValueHandling = NullValueHandling.Ignore)]
    public HttpOperationBindingMethod? Method { get; set; }

    /// <summary>
    /// A Schema object containing the definitions for each query parameter.
    /// This schema MUST be of type object and have a properties key.
    /// </summary>
    [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? Query { get; set; }

    /// <summary>
    /// The version of this binding. If omitted, "latest" MUST be assumed.
    /// </summary>
    [JsonProperty("bindingVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? BindingVersion { get; set; }
}


[JsonConverter(typeof(StringEnumConverter))]
public enum HttpOperationBindingType
{
    [EnumMember(Value = "request")]
    Request,

    [EnumMember(Value = "response")]
    Response,
}

[JsonConverter(typeof(StringEnumConverter))]
public enum HttpOperationBindingMethod
{
    [EnumMember(Value = "GET")]
    GET,

    [EnumMember(Value = "POST")]
    POST,

    [EnumMember(Value = "PUT")]
    PUT,

    [EnumMember(Value = "PATCH")]
    PATCH,

    [EnumMember(Value = "DELETE")]
    DELETE,

    [EnumMember(Value = "HEAD")]
    HEAD,

    [EnumMember(Value = "OPTIONS")]
    OPTIONS,

    [EnumMember(Value = "CONNECT")]
    CONNECT,

    [EnumMember(Value = "TRACE")]
    TRACE,
}
