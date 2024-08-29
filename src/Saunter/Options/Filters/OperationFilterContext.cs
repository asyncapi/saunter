using System.Reflection;
using Saunter.AttributeProvider.Attributes;

namespace Saunter.Options.Filters
{
    public class OperationFilterContext
    {
        public OperationFilterContext(MethodInfo method, OperationAttribute operation)
        {
            Method = method;
            Operation = operation;
        }

        public MethodInfo Method { get; }

        public OperationAttribute Operation { get; }
    }
}
