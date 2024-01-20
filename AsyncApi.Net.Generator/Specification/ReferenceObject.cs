namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// A simple object to allow referencing other components in the specification, internally and externally.
/// The Reference Object is defined by JSON Reference and follows the same structure, behavior, and rules.
/// A JSON Reference SHALL only be used to refer to a schema that is formatted in either JSON or YAML.
/// In the case of a YAML-formatted Schema, the JSON Reference SHALL be applied to the JSON representation of that schema.
/// The JSON representation SHALL be made by applying the conversion described [here](#format).
/// For this specification, reference resolution is done as defined by the JSON Reference specification and not by the JSON Schema specification.
/// </summary>
public class ReferenceObject
{
    /// <summary>
    /// Gets or sets the reference string.
    /// </summary>
    public string Ref { get; set; }
}
