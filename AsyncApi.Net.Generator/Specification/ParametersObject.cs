using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Describes a map of parameters included in a channel address.
/// This map MUST contain all the parameters used in the parent channel address.
/// </summary>
public class ParametersObject : Dictionary<string, ParameterObject>;
