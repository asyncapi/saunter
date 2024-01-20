using NJsonSchema;

using System;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Represents the root document object for the API specification.
/// It combines resource listing and API declaration together into one document.
/// </summary>
public class AsyncAPIObject
{
    public AsyncAPIObject(string id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets the AsyncAPI Specification version being used.
    /// It can be used by tooling Specifications and clients to interpret the version.
    /// </summary>
    /// <remarks>
    /// The structure shall be `major`.`minor`.`patch`, where `patch` versions
    /// must be compatible with the existing `major`.`minor` tooling.
    /// Typically patch versions will be introduced to address errors in the documentation,
    /// and tooling should typically be compatible with the corresponding `major`.`minor` (1.0.*).
    /// Patch versions will correspond to patches of this document.
    /// </remarks>
    public string AsyncAPI { get; } = "3.0.0";

    /// <summary>
    /// Gets or sets the identifier of the application the AsyncAPI document is defining.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets metadata about the API. The metadata can be used by the clients if needed.
    /// </summary>
    public InfoObject Info { get; } = new();

    /// <summary>
    /// Gets or sets connection details of servers.
    /// </summary>
    public ServersObject Servers { get; } = new();

    /// <summary>
    /// Gets or sets the default content type to use when encoding/decoding a message's payload.
    /// </summary>
    public string? DefaultContentType { get; set; }

    /// <summary>
    /// Gets or sets the channels used by this application.
    /// </summary>
    public ChannelsObject Channels { get; } = new();

    /// <summary>
    /// Gets or sets the operations this application MUST implement.
    /// </summary>
    public OperationsObject Operations { get; } = new();

    /// <summary>
    /// Gets or sets an element to hold various reusable objects for the specification.
    /// Everything that is defined inside this object represents a resource
    /// that MAY or MAY NOT be used in the rest of the document
    /// and MAY or MAY NOT be used by the implemented Application.
    /// </summary>
    public ComponentsObject Components { get; } = new();
}
