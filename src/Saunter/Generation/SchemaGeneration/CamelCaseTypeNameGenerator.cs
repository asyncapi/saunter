using System.Collections.Generic;
using NJsonSchema;

namespace Saunter.Generation.SchemaGeneration
{
    public class CamelCaseTypeNameGenerator : DefaultTypeNameGenerator
    {
        public override string Generate(JsonSchema schema, string typeNameHint, IEnumerable<string> reservedTypeNames)
        {
            return ToCamelCase(base.Generate(schema, typeNameHint, reservedTypeNames));
        }

        protected override string Generate(JsonSchema schema, string typeNameHint)
        {
            return ToCamelCase(base.Generate(schema, typeNameHint));
        }

        private string ToCamelCase(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return typeName;
            }

            return char.ToLower(typeName[0]) + typeName.Substring(1);
        }
    }
}
