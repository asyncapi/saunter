using Newtonsoft.Json.Schema;

namespace AsyncApiLibrary.Schema.v2;

public class Parameter
{
    /// <summary>
    /// A verbose explanation of the parameter.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Definition of the parameter.
    /// </summary>
    public JSchema? Schema { get; set; }

    /// <summary>
    /// A runtime expression that specifies the location of the parameter value.
    /// Even when a definition for the target field exists, it MUST NOT be used to validate
    /// this parameter but, instead, the schema property MUST be used.
    /// </summary>
    public string? Location { get; set; }
}
