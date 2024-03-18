using Saunter.AsyncApiSchema.v2;

namespace Saunter.Cli;

/// <summary>
/// DTO to create specification prototype
/// </summary>
/// <param name="Id">Identifier of the application the AsyncAPI document is defining.</param>
/// <param name="Info">Provides metadata about the API. The metadata can be used by the clients if needed.</param>
/// <param name="Servers">Provides connection details of servers.</param>
/// <param name="DefaultContentType">A string representing the default content type to use when encoding/decoding a message's payload.</param>
/// <param name="ExternalDocs">Additional external documentation.</param>
public record AsyncApiPrototype(
    string Id,
    Info? Info,
    Dictionary<string, Server>? Servers,
    string? DefaultContentType,
    ExternalDocumentation? ExternalDocs);
