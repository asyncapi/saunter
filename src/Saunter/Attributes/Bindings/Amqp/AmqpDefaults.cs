using Saunter.AsyncApiSchema.v2_0_0.Bindings.Amqp;

namespace Saunter.Attributes.Bindings.Amqp
{
    public static class AmqpDefaults
    {

        public static class ChannelBinding
        {
            public static AmqpChannelBindingIs Is { get; set; } = AmqpChannelBindingIs.RoutingKey;

            public static string ExchangeType { get; set; }

            public static string Name { get; set; }

            public static bool? AutoDelete { get; set; }

            public static bool? Durable { get; set; }

            public static bool? Exclusive { get; set; }
        }
    }
    
}