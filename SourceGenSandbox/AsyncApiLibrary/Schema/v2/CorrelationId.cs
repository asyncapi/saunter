namespace AsyncApiLibrary.Schema.v2;

public class CorrelationId
{
    public CorrelationId(string location)
    {
        Location = location ?? throw new ArgumentNullException(nameof(location));
    }

    /// <summary>
    /// An optional description of the identifier.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// A runtime expression that specifies the location of the correlation ID.
    /// </summary>
    public string Location { get; }

}