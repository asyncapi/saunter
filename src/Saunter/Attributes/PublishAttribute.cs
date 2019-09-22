using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PublishAttribute : Attribute
    {
        public string OperationId { get; set; }

        public Type HeadersType { get; set; }

        public Type PayloadType { get; set; }

        public string ContentType { get; set; } = "application/json";
    }
}