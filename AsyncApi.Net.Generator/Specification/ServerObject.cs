using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// An object representing a message broker, a server, or any other kind of computer program
/// capable of sending and/or receiving data. This object is used to capture details
/// such as URIs, protocols, and security configuration. Variable substitution can be used
/// so that some details, for example, usernames and passwords, can be injected by code generation tools.
/// </summary>
public class ServerObject
{
    /// <summary>
    /// Gets or sets the server host name. It MAY include the port.
    /// This field supports Server Variables. Variable substitutions will be made when a variable is named in {braces}.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The server host name.
    /// </remarks>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the protocol this server supports for connection.
    /// </summary>
    /// <remarks>
    /// **REQUIRED.** The protocol this server supports for connection.
    /// </remarks>
    public string Protocol { get; set; }

    /// <summary>
    /// Gets or sets the version of the protocol used for connection.
    /// For instance: AMQP 0.9.1, HTTP 2.0, Kafka 1.0.0, etc.
    /// </summary>
    public string ProtocolVersion { get; set; }

    /// <summary>
    /// Gets or sets the path to a resource in the host.
    /// This field supports Server Variables. Variable substitutions will be made when a variable is named in {braces}.
    /// </summary>
    public string Pathname { get; set; }

    /// <summary>
    /// Gets or sets an optional string describing the server.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets a human-friendly title for the server.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets a short summary of the server.
    /// </summary>
    public string Summary { get; set; }

    /// <summary>
    /// Gets or sets a map between a variable name and its value.
    /// The value is used for substitution in the server's `host` and `pathname` template.
    /// </summary>
    public Dictionary<string, ServerVariableObject> Variables { get; set; }

    /// <summary>
    /// Gets or sets a declaration of which security schemes can be used with this server.
    /// </summary>
    public List<SecuritySchemeObject> Security { get; set; }

    /// <summary>
    /// Gets or sets a list of tags for logical grouping and categorization of servers.
    /// </summary>
    public TagsObject Tags { get; set; }

    /// <summary>
    /// Gets or sets additional external documentation for this server.
    /// </summary>
    public ExternalDocumentationObject ExternalDocs { get; set; }

    /// <summary>
    /// Gets or sets protocol-specific definitions for the server.
    /// </summary>
    public ServerBindingsObject Bindings { get; set; }
}
