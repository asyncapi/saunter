using System.Collections.Generic;

using NJsonSchema;

namespace AsyncApi.Net.Generator.Tests.Utils;

public static class NJsonSchemaExtensions
{
    public static IDictionary<string, JsonSchema> MergeAllProperties(this JsonSchema s)
    {
        Dictionary<string, JsonSchema> result = [];
        foreach (KeyValuePair<string, JsonSchemaProperty> property in s.ActualProperties)
        {
            result[property.Key] = property.Value;
        }

        foreach (JsonSchema sub in s.AllInheritedSchemas)
        {
            foreach (KeyValuePair<string, JsonSchemaProperty> property in sub.Properties)
            {
                result[property.Key] = property.Value;
            }
        }

        return result;
    }
}
