namespace AsyncApiLibrary.Schema.v2;

public class Tag
{
    /// <summary>
    /// The name of the tag.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// A short description for the tag. CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional external documentation for this tag.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    public static implicit operator Tag(string s) => new() { Name = s };
}