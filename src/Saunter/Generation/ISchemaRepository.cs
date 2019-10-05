using System;
using System.Collections.Generic;
using System.Linq;
using NJsonSchema;
using Saunter.AsyncApiSchema.v2_0_0;

namespace Saunter.Generation
{
    public interface ISchemaRepository
    {
        IDictionary<ComponentFieldName, JsonSchema> Schemas { get; }

        Reference GetOrAdd(Type type, string schemaId, Func<JsonSchema> factory);
    }
    
    public class SchemaRepository : ISchemaRepository
    {
        private Dictionary<Type, string> _reservedIds = new Dictionary<Type, string>();
        
        public IDictionary<ComponentFieldName, JsonSchema> Schemas { get; } = new Dictionary<ComponentFieldName, JsonSchema>();
        
        public Reference GetOrAdd(Type type, string schemaId, Func<JsonSchema> factory)
        {
            if (!_reservedIds.TryGetValue(type, out var reservedId))
            {
                // First invocation of the factoryMethod for this type - reserve the provided schemaId first, and then invoke the factory method.
                // Reserving the id first ensures that the factoryMethod will only be invoked once for a given type, even in recurrsive scenarios.
                // If subsequent calls are made for the same type, a simple reference will be created instead.
                ReserveIdFor(type, schemaId);
                Schemas.Add(schemaId, factory());
            }
            else
            {
                schemaId = reservedId;
            }
            
            return new Reference(schemaId, ReferenceType.Schema);
        }

        private void ReserveIdFor(Type type, string schemaId)
        {
            if (_reservedIds.ContainsValue(schemaId))
            {
                var reservedType = _reservedIds.First(entry => entry.Value == schemaId).Key;
                
                throw new InvalidOperationException($"Can't use schemaId '{schemaId}' for type {type}'." +
                                                    $" The same schemaId is already used by type '{reservedType}'");
            }
            
            _reservedIds.Add(type, schemaId);
        }
    }
}