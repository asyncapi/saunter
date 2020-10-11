using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Generation.SchemaGeneration;

namespace Saunter.Generation.Filters
{
    public interface OperationFilter
    {
        void Apply(Operation publishOperation, OperationFilterContext context);
    }

    public class OperationFilterContext
    {
        public OperationFilterContext(MethodInfo method, ISchemaRepository schemaRepository, OperationAttribute operation)
        {
            Method = method;
            SchemaRepository = schemaRepository;
            Operation = operation;
        }
        
        public MethodInfo Method { get; }

        public ISchemaRepository SchemaRepository { get; }
        
        public OperationAttribute Operation { get; }
    }
}