using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class DiscriminatorAttribute : Attribute
    {
        public DiscriminatorAttribute(string propertyName) => PropertyName = propertyName;

        public string PropertyName { get; }
    }
}