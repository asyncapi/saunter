using System;

namespace Saunter.Attributes
{
    [Obsolete("add DiscriminatorSchemaProcessor to JsonSchemaGeneratorSettings", true)]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class DiscriminatorAttribute : Attribute
    {
        public DiscriminatorAttribute(string propertyName) => PropertyName = propertyName;

        public string PropertyName { get; }
    }
}