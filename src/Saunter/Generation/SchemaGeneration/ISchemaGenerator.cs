using System;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.SchemaGeneration
{
    public interface ISchemaGenerator
    {
        /// <summary>
        /// Generate a schema, save it in the <paramref name="schemaRepository"/>, and return a reference to it.
        /// </summary>
        Reference GenerateSchema(Type type, ISchemaRepository schemaRepository);
    }

    // public class NJsonSchemaGenerator : ISchemaGenerator
    // {
    //     private static readonly JsonSchemaGeneratorSettings Settings = new JsonSchemaGeneratorSettings
    //     {
    //         SerializerSettings = new JsonSerializerSettings
    //         {
    //             ContractResolver = new CamelCasePropertyNamesContractResolver(), // todo: this has to be controllable...
    //         },
    //         // otherwise each schema has a property `"additionalProperties": false` which seems to break the current version of the AsyncAPI Playground
    //         AlwaysAllowAdditionalObjectProperties = true, 
    //         
    //     };
    //     
    //     public Reference GenerateSchema(Type type, ISchemaRepository schemaRepository)
    //     {
    //         var schemaId = type.Name; // todo should be able to customize
    //
    //         var schemaReference = schemaRepository.GetOrAdd(type, schemaId, () => JsonSchema.FromType(type, Settings));
    //
    //         return schemaReference;
    //     }
    // }

    public class NoOpSchemaGenerator : ISchemaGenerator
    {
        private static readonly Reference r = new Reference("fake", ReferenceType.Schema);
        
        public Reference GenerateSchema(Type type, ISchemaRepository schemaRepository)
        {
            return r;
        }
    }
}