using System.Collections.Generic;

using Newtonsoft.Json;

using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

public class Server
{
    /// <summary>
    /// REQUIRED. The server host name. It MAY include the port.
    /// This field supports Server Variables.
    /// Variable substitutions will be made when a variable is named in {braces}.
    /// </summary>
    [JsonProperty("host")]
    public required string Host { get; set; }

    /// <summary>
    /// The protocol this URL supports for connection.
    /// Supported protocol include, but are not limited to: amqp, amqps, http, https,
    /// jms, kafka, kafka-secure, mqtt, secure-mqtt, stomp, stomps, ws, wss.
    /// </summary>
    [JsonProperty("protocol")]
    public required string Protocol { get; set; }

    /// <summary>
    /// The version of the protocol used for connection.
    /// For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
    /// </summary>
    [JsonProperty("protocolVersion", NullValueHandling = NullValueHandling.Ignore)]
    public string? ProtocolVersion { get; set; }

    /// <summary>
    /// The path to a resource in the host.
    /// This field supports Server Variables.
    /// Variable substitutions will be made when a variable is named in {braces}.
    /// </summary>
    [JsonProperty("pathname", NullValueHandling = NullValueHandling.Ignore)]
    public string? PathName { get; set; }

    /// <summary>
    /// An optional string describing the host designated by the URL.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// A human-friendly title for the server.
    /// </summary>
    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string? Title { get; set; }

    /// <summary>
    /// A short summary of the server.
    /// </summary>
    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string? Summary { get; set; }

    /// <summary>
    /// A map between a variable name and its value.
    /// The value is used for substitution in the server's URL template.
    /// </summary>
    [JsonProperty("variables", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, ServerVariable>? Variables { get; set; }

    /// <summary>
    /// A declaration of which security mechanisms can be used with this server.
    /// The list of values includes alternative security requirement objects
    /// that can be used. Only one of the security requirement objects need to
    /// be satisfied to authorize a connection or operation.
    /// </summary>
    [JsonProperty("security", NullValueHandling = NullValueHandling.Ignore)]
    public List<Dictionary<string, List<string>>>? Security { get; set; }

    /// <summary>
    /// A list of tags used by the specification with additional metadata.
    /// Each tag name in the list MUST be unique.
    /// </summary>
    [JsonProperty("tags")]
    public HashSet<Tag> Tags { get; set; } = [];

    /// <summary>
    /// Additional external documentation.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IServerBindings? Bindings { get; set; }
}

public class ServerVariable
{
    /// <summary>
    /// An enumeration of string values to be used if the substitution options are from a limited set.
    /// </summary>
    [JsonProperty("enum", NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? Enum { get; set; }

    /// <summary>
    /// The default value to use for substitution, and to send, if an alternate value is not supplied.
    /// </summary>
    [JsonProperty("default", NullValueHandling = NullValueHandling.Ignore)]
    public string? Default { get; set; }

    /// <summary>
    /// An optional description for the server variable.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// An array of examples of the server variable.
    /// </summary>
    [JsonProperty("examples", NullValueHandling = NullValueHandling.Ignore)]
    public List<string>? Examples { get; set; }
}
