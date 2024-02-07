using AsyncApiLibrary.Schema.v2.Bindings;

namespace AsyncApiLibrary.Schema.v2;

public class Server
{
    public Server(string url, string protocol)
    {
        Url = url;
        Protocol = protocol;
    }

    /// <summary>
    /// A URL to the target host.
    /// This URL supports Server Variables and MAY be relative, to indicate that the host
    /// location is relative to the location where the AsyncAPI document is being served.
    /// Variable substitutions will be made when a variable is named in { brackets }.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// The protocol this URL supports for connection.
    /// Supported protocol include, but are not limited to: amqp, amqps, http, https,
    /// jms, kafka, kafka-secure, mqtt, secure-mqtt, stomp, stomps, ws, wss.
    /// </summary>
    public string Protocol { get; set; }

    /// <summary>
    /// The version of the protocol used for connection.
    /// For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
    /// </summary>
    public string? ProtocolVersion { get; set; }

    /// <summary>
    /// An optional string describing the host designated by the URL.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A map between a variable name and its value.
    /// The value is used for substitution in the server's URL template.
    /// </summary>
    public Dictionary<string, ServerVariable>? Variables { get; set; }

    /// <summary>
    /// A declaration of which security mechanisms can be used with this server.
    /// The list of values includes alternative security requirement objects
    /// that can be used. Only one of the security requirement objects need to
    /// be satisfied to authorize a connection or operation.
    /// </summary>
    public List<Dictionary<string, List<string>>>? Security { get; set; }

    /// <summary>
    ///A list of tags for logical grouping and categorization of servers.
    /// </summary>
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// A map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the server.
    /// </summary>
    public ServerBindings? Bindings { get; set; }
}

public class ServerVariable
{
    /// <summary>
    /// An enumeration of string values to be used if the substitution options are from a limited set.
    /// </summary>
    public List<string>? Enum { get; set; }

    /// <summary>
    /// The default value to use for substitution, and to send, if an alternate value is not supplied.
    /// </summary>
    public string? Default { get; set; }

    /// <summary>
    /// An optional description for the server variable.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// An array of examples of the server variable.
    /// </summary>
    public List<string>? Examples { get; set; }
}
