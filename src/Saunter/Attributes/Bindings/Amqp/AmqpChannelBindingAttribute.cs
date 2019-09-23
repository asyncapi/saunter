using System;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;

namespace Saunter.Attributes.Bindings.Amqp
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AmqpChannelBindingAttribute : Attribute
    {
        public AmqpChannelIs Is { get; }
        
        public string ExchangeName { get; set; }
        
        public AmqpExchangeType ExchangeType { get; set; }
        
        public AmqpChannelBindingAttribute(AmqpChannelIs @is)
        {
            Is = @is;
        }

        public AmqpChannelBinding ToChannelBinding()
        {
            return new AmqpChannelBinding
            {
                Is = Is,
                Exchange = new AmqpChannelExchange
                {
                    Name = ExchangeName,
                    Type = ExchangeType,
                }
            };
        }
    }
}