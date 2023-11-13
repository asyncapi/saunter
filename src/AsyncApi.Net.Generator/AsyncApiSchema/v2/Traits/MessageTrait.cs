using NJsonSchema;
using System.Collections.Generic;
using Newtonsoft.Json;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2.Traits;

/// <summary>
/// Can be either a <see cref="MessageTrait"/> or <see cref="MessageTraitReference"/> reference to a message trait.
/// </summary>
public interface IMessageTrait { }

/// <summary>
/// A reference to a MessageTrait within the AsyncAPI components.
/// </summary>
public class MessageTraitReference : Reference, IMessageTrait
{
    public MessageTraitReference(string id) : base(id, "#/components/messageTraits/{0}") { }
}

public class MessageTrait : IMessageTrait
{
    /// <summary>
    /// Schema definition of the application headers.
    /// Schema MUST be of type "object". It MUST NOT define the protocol headers.
    /// </summary>
    [JsonProperty("headers")]
    public JsonSchema? Headers { get; set; }

    /// <summary>
    /// Definition of the correlation ID used for message tracing or matching.
    /// </summary>
    [JsonProperty("correlationId")]
    public ICorrelationId? CorrelationId { get; set; }

    /// <summary>
    /// A string containing the name of the schema format/language used to define
    /// the message payload. If omitted, implementations should parse the payload as a Schema object.
    /// </summary>
    [JsonProperty("schemaFormat")]
    public string? SchemaFormat { get; set; }

    /// <summary>
    /// The content type to use when encoding/decoding a message's payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// When omitted, the value MUST be the one specified on the defaultContentType field.
    /// </summary>
    [JsonProperty("contentType")]
    public string? ContentType { get; set; }

    /// <summary>
    /// A machine-friendly name for the message.
    /// </summary>
    [JsonProperty("name")]
    public string? Name { get; set; }

    /// <summary>
    /// A human-friendly title for the message.
    /// </summary>
    [JsonProperty("title")]
    public string? Title { get; set; }

    /// <summary>
    /// A short summary of what the message is about.
    /// </summary>
    [JsonProperty("summary")]
    public string? Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the message.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description")]
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control.
    /// Tags can be used for logical grouping of messages.
    /// </summary>
    [JsonProperty("tags")]
    public ISet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this message.
    /// </summary>
    [JsonProperty("externalDocs")]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol
    /// and the values describe protocol-specific definitions for the message.
    /// </summary>
    [JsonProperty("bindings")]
    public IMessageBindings? Bindings { get; set; }

    /// <summary>
    /// An array with examples of valid message objects.
    /// </summary>
    [JsonProperty("examples")]
    public IList<IDictionary<string, object>>? Examples { get; set; }
}