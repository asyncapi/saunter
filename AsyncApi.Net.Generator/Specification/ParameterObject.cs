using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a parameter included in a channel address.
/// </summary>
public class ParameterObject
{
    /// <summary>
    /// An enumeration of string values to be used if the substitution options are from a limited set.
    /// </summary>
    public List<string> Enum { get; set; }

    /// <summary>
    /// The default value to use for substitution, and to send, if an alternate value is not supplied.
    /// </summary>
    public string Default { get; set; }

    /// <summary>
    /// An optional description for the parameter.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// An array of examples of the parameter value.
    /// </summary>
    public List<string> Examples { get; set; }

    /// <summary>
    /// A runtime expression that specifies the location of the parameter value.
    /// </summary>
    public string Location { get; set; }
}
