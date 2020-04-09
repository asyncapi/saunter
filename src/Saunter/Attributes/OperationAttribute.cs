using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class OperationAttribute : Attribute
    {
        public OperationAttribute(Type messagePayloadType)
        {
            MessagePayloadType = messagePayloadType;
        }

        public OperationType OperationType { get; set; }
        public string Summary { get; set; }

        public Type MessagePayloadType { get; }

        public string OperationId { get; set; }
        
        public string Description { get; set; }
    }

    public class PublishOperationAttribute : OperationAttribute
    {
        public PublishOperationAttribute(Type messagePayloadType) : base(messagePayloadType)
        {
            OperationType = OperationType.Publish;
        }
    }

    public class SubscribeOperationAttribute : OperationAttribute
    {
        public SubscribeOperationAttribute(Type messagePayloadType) : base(messagePayloadType)
        {
            OperationType = OperationType.Publish;
        }
    }

    public enum OperationType
    {
        Publish,
        Subscribe
    }
}
