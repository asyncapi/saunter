using System;
using System.Collections.Generic;

namespace Saunter.Generation
{
    public interface ISchemaFilter
    {
        void Apply(AsyncApiSchema.v2_0_0.AsyncApiSchema schema, SchemaFilterContext context);
    }

    public class SchemaFilterContext
    {
        public SchemaFilterContext(IEnumerable<Type> asyncApiTypes, ISchemaRepository schemaRepository)
        {
            AsyncApiTypes = asyncApiTypes;
            SchemaRepository = schemaRepository;
        }
        
        public IEnumerable<Type> AsyncApiTypes { get; }
        
        public ISchemaRepository SchemaRepository { get; }
    }
}