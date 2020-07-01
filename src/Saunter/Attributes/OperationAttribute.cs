using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public abstract class OperationAttribute : Attribute
    {
        public OperationType OperationType { get; protected set; }
        
        public Type MessagePayloadType { get; protected set; }
        
        public string Summary { get; set; }

        public string OperationId { get; set; }
        
        public string Description { get; set; }
    }

    public class PublishOperationAttribute : OperationAttribute
    {
        public PublishOperationAttribute(Type messagePayloadType)
        {
            OperationType = OperationType.Publish;
            MessagePayloadType = messagePayloadType;
        }

        public PublishOperationAttribute()
        {
            OperationType = OperationType.Publish;
        }
    }

    public class SubscribeOperationAttribute : OperationAttribute
    {
        public SubscribeOperationAttribute(Type messagePayloadType)
        {
            OperationType = OperationType.Subscribe;
            MessagePayloadType = messagePayloadType;
        }

        public SubscribeOperationAttribute()
        {
            OperationType = OperationType.Subscribe;
        }
    }

    public enum OperationType
    {
        Publish,
        Subscribe
    }
    
    
}
