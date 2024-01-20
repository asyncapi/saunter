using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// An object representing a Server Variable for server URL template substitution.
/// </summary>
public class ServerVariableObject
{
    /// <summary>
    /// Gets or sets an enumeration of string values to be used if the substitution options are from a limited set.
    /// </summary>
    public List<string> Enum { get; set; }

    /// <summary>
    /// Gets or sets the default value to use for substitution, and to send if an alternate value is not supplied.
    /// </summary>
    public string Default { get; set; }

    /// <summary>
    /// Gets or sets an optional description for the server variable.
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets an array of examples of the server variable.
    /// </summary>
    public List<string> Examples { get; set; }
}
