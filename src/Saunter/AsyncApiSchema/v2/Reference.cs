using System;
using System.Globalization;

using Newtonsoft.Json;

namespace Saunter.AsyncApiSchema.v2;

/// <summary>
/// A reference to some other object within the AsyncAPI document. 
/// </summary>
public class Reference
{
    public Reference(string id, string path)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    private readonly string _path;

    [JsonProperty("$ref")]
    public string Ref => string.Format(CultureInfo.InvariantCulture, _path, Id);

    [JsonIgnore()]
    public string Id { get; }
}
