using NJsonSchema;
using Newtonsoft.Json;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <summary>
/// Can be either a <see cref="Parameter"/> or a <see cref="ParameterReference"/> reference to a parameter.
/// </summary>
public interface IParameter { }

/// <summary>
/// A reference to a Parameter within the AsyncAPI components.
/// </summary>
public class ParameterReference : Reference, IParameter
{
    private readonly AsyncApiDocument _document;
    public ParameterReference(string id, AsyncApiDocument document) : base(id, "#/components/parameters/{0}")
    {
        _document = document;
    }

    [JsonIgnore]
    public Parameter Value => _document.Components.Parameters[Id];
}

public class Parameter : IParameter
{
    /// <summary>
    /// A verbose explanation of the parameter.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// Definition of the parameter.
    /// </summary>
    [JsonProperty("schema")]
    public JsonSchema? Schema { get; set; }

    /// <summary>
    /// A runtime expression that specifies the location of the parameter value.
    /// Even when a definition for the target field exists, it MUST NOT be used to validate
    /// this parameter but, instead, the schema property MUST be used.
    /// </summary>
    [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
    public string? Location { get; set; }

    [JsonIgnore]
    internal string? Name { get; set; }
}