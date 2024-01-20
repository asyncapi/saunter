using NJsonSchema;

using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Message Example Object represents an example of a Message Object and MUST contain either headers and/or payload fields.
/// </summary>
public class MessageExampleObject
{
    public MessageExampleObject(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets or sets the value of this field. It MUST validate against the Message Object's headers field.
    /// </summary>
    public Dictionary<string, JsonSchema>? Headers { get; set; }

    /// <summary>
    /// Gets or sets the value of this field. It MUST validate against the Message Object's payload field.
    /// </summary>
    public Dictionary<string, JsonSchema>? Payload { get; set; }

    /// <summary>
    /// Gets or sets a machine-friendly name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets a short summary of what the example is about.
    /// </summary>
    public string? Summary { get; set; }
}
