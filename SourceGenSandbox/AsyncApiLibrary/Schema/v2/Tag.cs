namespace AsyncApiLibrary.Schema.v2;

public class Tag
{
    public Tag(string name)
    {
        Name = name;
    }

    /// <summary>
    /// The name of the tag.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A short description for the tag. CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Additional external documentation for this tag.
    /// </summary>
    public ExternalDocumentation? ExternalDocs { get; set; }

    public static implicit operator Tag(string s) => new(s);
}