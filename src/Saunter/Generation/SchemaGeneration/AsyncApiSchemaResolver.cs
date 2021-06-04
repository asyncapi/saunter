using System;
using System.Linq;
using NJsonSchema;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation.SchemaGeneration
{
    public class AsyncApiSchemaResolver : JsonSchemaResolver
    {
        private readonly AsyncApiDocument _document;
        private readonly JsonSchemaGeneratorSettings _settings;

        public AsyncApiSchemaResolver(AsyncApiDocument document, JsonSchemaGeneratorSettings settings) 
            : base(document, settings)
        {
            _document = document;
            _settings = settings;
        }

        public override void AppendSchema(JsonSchema schema, string typeNameHint)
        {
            if (schema == null)
                throw new ArgumentNullException(nameof(schema));
            if (schema == RootObject)
                throw new ArgumentException("The root schema cannot be appended.");
            
            if (!_document.Components.Schemas.Values.Contains(schema))
            {
                // TODO: how to use our SchemaIdFactory here? Do we need to?
                var schemaId = _settings.TypeNameGenerator.Generate(schema, typeNameHint, _document.Components.Schemas.Keys.Select(k => k.ToString()));

                schema.Id = schemaId;

                if (!string.IsNullOrEmpty(schemaId) && !_document.Components.Schemas.ContainsKey(schemaId))
                    _document.Components.Schemas.Add(new ComponentFieldName(schemaId), schema);
                else
                    _document.Components.Schemas.Add(new ComponentFieldName("ref_" + Guid.NewGuid().ToString().Replace("-", "_")), schema);
            }
        }
    }
}