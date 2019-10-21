using System;
using System.Collections.Generic;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation.Filters
{
    public interface IDocumentFilter
    {
        void Apply(AsyncApiDocument document, DocumentFilterContext context);
    }

    public class DocumentFilterContext
    {
        public DocumentFilterContext(IEnumerable<Type> asyncApiTypes, ISchemaRepository schemaRepository)
        {
            AsyncApiTypes = asyncApiTypes;
            SchemaRepository = schemaRepository;
        }
        
        public IEnumerable<Type> AsyncApiTypes { get; }
        
        public ISchemaRepository SchemaRepository { get; }
    }
}