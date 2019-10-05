using System.Reflection;
using Saunter.AsyncApiSchema.v2_0_0;
using Saunter.Attributes;

namespace Saunter.Generation
{
    public interface IPublishOperationFilter
    {
        void Apply(Operation publishOperation, PublishOperationFilterContext context);
    }

    public class PublishOperationFilterContext
    {
        public PublishOperationFilterContext(MethodInfo method, ISchemaRepository schemaRepository, PublishOperationAttribute publishOperation)
        {
            Method = method;
            SchemaRepository = schemaRepository;
            PublishOperation = publishOperation;
        }
        
        public MethodInfo Method { get; }

        public ISchemaRepository SchemaRepository { get; }
        
        public PublishOperationAttribute PublishOperation { get; }
    }
}