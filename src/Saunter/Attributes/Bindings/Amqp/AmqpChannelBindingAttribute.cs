using System;
using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;

namespace Saunter.Attributes.Bindings.Amqp
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AmqpChannelBindingAttribute : Attribute
    {
        // common properties
        public AmqpChannelBindingIs Is { get; set; } = AmqpDefaults.ChannelBinding.Is;

        public string Name { get; set; } = AmqpDefaults.ChannelBinding.Name;
        
        public bool? AutoDelete { get; set; } = AmqpDefaults.ChannelBinding.AutoDelete;

        public bool? Durable { get; set; } = AmqpDefaults.ChannelBinding.Durable;
        
        // exchange only
        public string ExchangeType { get; set; } = AmqpDefaults.ChannelBinding.ExchangeType;
        
        // queue only
        public bool? Exclusive { get; set; } = AmqpDefaults.ChannelBinding.Exclusive;
        
        public AmqpChannelBinding ToChannelBinding()
        {
            var binding = new AmqpChannelBinding
            {
                Is = Is,
            };

            switch (Is)
            {
                case AmqpChannelBindingIs.RoutingKey:
                    binding.Exchange = new AmqpChannelBindingExchange
                    {
                        Name = Name,
                        Type = ExchangeType,
                        AutoDelete = AutoDelete,
                        Durable = Durable,
                    };
                    break;

                case AmqpChannelBindingIs.Queue:
                    binding.Queue = new AmqpChannelBindingQueue
                    {
                        Name = Name,
                        Durable = Durable,
                        AutoDelete = AutoDelete,
                        Exclusive = Exclusive,
                    };
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return binding;
        }
    }
}