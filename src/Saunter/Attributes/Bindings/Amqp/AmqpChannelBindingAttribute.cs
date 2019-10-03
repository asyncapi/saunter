using System;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;

namespace Saunter.Attributes.Bindings.Amqp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AmqpChannelBindingAttribute : Attribute
    {
        // common properties
        public AmqpChannelBindingIs? Is { get; set; }

        public string Name { get; set; }
        
        public bool? AutoDelete { get; set; }

        public bool? Durable { get; set; }
        
        // exchange only
        public string ExchangeType { get; set; }
        
        // queue only
        public bool? Exclusive { get; set; }
    }
}