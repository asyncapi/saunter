using System.Reflection;

using NJsonSchema.Generation;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.Attributes;

namespace AsyncApi.Net.Generator.Generation.Filters;

public interface IOperationFilter
{
    void Apply(Operation operation, OperationFilterContext context);
}

public class OperationFilterContext
{
    public OperationFilterContext(MethodInfo method, JsonSchemaResolver schemaResolver, JsonSchemaGenerator schemaGenerator, OperationAttribute operation)
    {
        Method = method;
        SchemaResolver = schemaResolver;
        SchemaGenerator = schemaGenerator;
        Operation = operation;
    }

    public MethodInfo Method { get; }

    public JsonSchemaResolver SchemaResolver { get; }

    public JsonSchemaGenerator SchemaGenerator { get; }

    public OperationAttribute Operation { get; }
}