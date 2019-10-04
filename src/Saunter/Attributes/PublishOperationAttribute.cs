using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PublishOperationAttribute : Attribute
    {
        public PublishOperationAttribute(Type payloadType)
        {
            PayloadType = payloadType;
        }
        
        public string Summary { get; set; }

        public Type PayloadType { get; }
    }
}