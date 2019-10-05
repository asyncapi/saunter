using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PublishOperationAttribute : Attribute
    {
        public PublishOperationAttribute(Type messagePayloadType)
        {
            MessagePayloadType = messagePayloadType;
        }
        
        public string Summary { get; set; }

        public Type MessagePayloadType { get; }

        public string OperationId { get; set; }
        
        public string Description { get; set; }
    }
}