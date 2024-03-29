﻿using System.Collections.Generic;
using NJsonSchema;

namespace Saunter.Tests.Utils
{
    public static class NJsonSchemaExtensions
    {
        public static IDictionary<string, JsonSchema> MergeAllProperties(this JsonSchema s)
        {
            var result = new Dictionary<string, JsonSchema>();
            foreach (var property in s.ActualProperties)
            {
                result[property.Key] = property.Value;
            }

            foreach (var sub in s.AllInheritedSchemas)
            {
                foreach (var property in sub.Properties)
                {
                    result[property.Key] = property.Value;
                }
            }

            return result;
        }
    }
}
