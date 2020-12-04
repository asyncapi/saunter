using System;
using System.Collections.Generic;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.SchemaGeneration
{
    [Obsolete("Saunter now uses NJsonSchema.Generation.JsonSchemaResolver", true)]
    public interface ISchemaRepository
    {
        IDictionary<ComponentFieldName, Schema> Schemas { get; }

        ISchema GetOrAdd(Type type, string schemaId, Func<Schema> factory);
    }
}