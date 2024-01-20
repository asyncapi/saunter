using NJsonSchema;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// The Multi Format Schema Object represents a schema definition.
/// It supports multiple schema formats or languages (e.g., JSON Schema, Avro, etc.).
/// </summary>
public class MultiFormatSchemaObject
{
    public MultiFormatSchemaObject(string schemaFormat, JsonSchema schema)
    {
        SchemaFormat = schemaFormat;
        Schema = schema;
    }

    /// <summary>
    /// Gets or sets the name of the schema format that is used to define the information.
    /// </summary>
    public string SchemaFormat { get; set; }

    /// <summary>
    /// Gets or sets the definition of the message payload.
    /// It can be of any type but defaults to Schema Object.
    /// It MUST match the schema format defined in SchemaFormat, including the encoding type.
    /// Non-JSON-based schemas (e.g., Protobuf or XSD) MUST be inlined as a string.
    /// </summary>
    public JsonSchema Schema { get; set; }
}
