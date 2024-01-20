namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// An object that specifies an identifier at design time that can be used for message tracing and correlation.
/// </summary>
public class CorrelationIdObject
{
    public CorrelationIdObject(string location)
    {
        Location = location;
    }

    /// <summary>
    /// Gets or sets an optional description of the identifier.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets a required runtime expression that specifies the location of the correlation ID.
    /// </summary>
    public string Location { get; set; }
}
