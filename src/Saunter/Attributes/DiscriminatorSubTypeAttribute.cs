using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface, AllowMultiple = true)]
    public class DiscriminatorSubTypeAttribute : Attribute
    {
        public DiscriminatorSubTypeAttribute(Type subType) => SubType = subType;

        public Type SubType { get; }

        public string DiscriminatorValue { get; set; }
    }
}