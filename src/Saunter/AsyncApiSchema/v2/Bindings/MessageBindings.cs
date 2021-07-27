using Newtonsoft.Json;
using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using Saunter.AsyncApiSchema.v2.Bindings.Kafka;

namespace Saunter.AsyncApiSchema.v2.Bindings
{
    /// <summary>
    /// MessageBindings can be either a the bindings or a reference to the bindings.
    /// </summary>
    public interface IMessageBindings {}

    /// <summary>
    /// A reference to the MessageBindings within the AsyncAPI components.
    /// </summary>
    public class MessageBindingsReference : Reference, IMessageBindings
    {
        public MessageBindingsReference(string id) : base(id, "#/components/messageBindings/{0}") { }
    }

    public class MessageBindings : IMessageBindings
    {
        [JsonProperty("amqp")]
        public AmqpMessageBinding Amqp { get; set; }

        [JsonProperty("http")]
        public HttpMessageBinding Http { get; set; }

        [JsonProperty("kafka")]
        public KafkaMessageBinding Kafka { get; set; }
    }
}