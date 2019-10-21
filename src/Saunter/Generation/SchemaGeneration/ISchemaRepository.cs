using System;
using System.Collections.Generic;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.SchemaGeneration
{
    public interface ISchemaRepository
    {
        IDictionary<ComponentFieldName, Schema> Schemas { get; }

        ISchema GetOrAdd(Type type, string schemaId, Func<Schema> factory);
    }
}