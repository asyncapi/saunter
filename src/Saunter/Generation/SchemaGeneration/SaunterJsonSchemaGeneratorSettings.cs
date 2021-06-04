using NJsonSchema.Generation;

namespace Saunter.Generation.SchemaGeneration
{
    public class SaunterJsonSchemaGeneratorSettings : JsonSchemaGeneratorSettings
    {
        public SaunterJsonSchemaGeneratorSettings()
        {
            TypeNameGenerator = new CamelCaseTypeNameGenerator();
        }
    }
}
