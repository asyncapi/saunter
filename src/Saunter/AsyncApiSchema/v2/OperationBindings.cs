using Saunter.AsyncApiSchema.v2.Bindings.Amqp;
using Saunter.AsyncApiSchema.v2.Bindings.Http;
using System.Text.Json.Serialization;

namespace Saunter.AsyncApiSchema.v2 {
    public class OperationBindings
    {         
        [JsonPropertyName("http")]
        public HttpOperationBinding Http { get; set; }

        [JsonPropertyName("amqp")]
        public AmqpOperationBinding Amqp { get; set; }
    }
}