using System;
using System.Collections.Generic;
using NJsonSchema.Generation;
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
        public DocumentFilterContext(IEnumerable<Type> asyncApiTypes, JsonSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator)
        {
            AsyncApiTypes = asyncApiTypes;
            SchemaResolver = schemaResolver;
            SchemaGenerator = schemaGenerator;
        }
        
        public IEnumerable<Type> AsyncApiTypes { get; }

        public JsonSchemaResolver SchemaResolver { get; }

        public JsonSchemaGenerator SchemaGenerator { get; }

    }
}