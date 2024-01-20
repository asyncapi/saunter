using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// This object contains information about the message representation in Kafka.
/// </summary>
public class KafkaMessageBinding
{
    // The message key.
    // NOTE: You can also use the reference object way.
    public JsonSchema? Key { get; set; }

    // If a Schema Registry is used when performing this operation, tells where the id of schema is stored (e.g. 'header' or 'payload').
    // OPTIONAL. MUST NOT be specified if schemaRegistryUrl is not specified at the Server level.
    public string? SchemaIdLocation { get; set; }

    // Number of bytes or vendor specific values when schema id is encoded in payload (e.g 'confluent'/ 'apicurio-legacy' / 'apicurio-new').
    // OPTIONAL. MUST NOT be specified if schemaRegistryUrl is not specified at the Server level.
    public string? SchemaIdPayloadEncoding { get; set; }

    // Freeform string for any naming strategy class to use. Clients should default to the vendor default if not supplied.
    // OPTIONAL. MUST NOT be specified if schemaRegistryUrl is not specified at the Server level.
    public string? SchemaLookupStrategy { get; set; }

    // The version of this binding. If omitted, "latest" MUST be assumed.
    public string? BindingVersion { get; set; }
}
