namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// An object that specifies where an operation has to send the reply.
/// For specifying and computing the location of a reply address, a <see cref="RuntimeExpression"/> is used.
/// </summary>
public class OperationReplyAddressObject
{
    /// <summary>
    /// Gets or sets an optional description of the address.
    /// <see cref="CommonMark syntax"/> can be used for rich text representation.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the <c>REQUIRED</c> runtime expression that specifies the location of the reply address.
    /// </summary>
    public string Location { get; set; }
}
