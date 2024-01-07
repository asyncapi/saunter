using NJsonSchema;
using System.Collections.Generic;
using Newtonsoft.Json;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Bindings;
using AsyncApi.Net.Generator.AsyncApiSchema.v2.Traits;

namespace AsyncApi.Net.Generator.AsyncApiSchema.v2;

/// <remarks>
/// Message can either be a list of messages, a single message, or a reference to a message.
/// </remarks>
public interface IMessage { }

/// <summary>
/// A reference to a Message within the AsyncAPI components.
/// </summary>
public class MessageReference : Reference, IMessage
{
    public MessageReference(string id) : base(id, "#/components/messages/{0}") { }
}

public class Messages : IMessage
{
    [JsonProperty("oneOf")]
    public List<IMessage> OneOf { get; set; } = [];
}

public class Message : IMessage
{
    /// <summary>
    /// Unique string used to identify the message. The id MUST be unique among all messages
    /// described in the API. The messageId value is case-sensitive. Tools and libraries MAY
    /// use the messageId to uniquely identify a message, therefore, it is RECOMMENDED to
    /// follow common programming naming conventions.
    /// </summary>
    [JsonProperty("messageId", NullValueHandling = NullValueHandling.Ignore)]
    public string? MessageId { get; set; }

    /// <summary>
    /// Schema definition of the application headers. Schema MUST be of type “object”.
    /// It MUST NOT define the protocol headers.
    /// TODO: add ref type
    /// </summary>
    [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? Headers { get; set; }

    /// <summary>
    /// Definition of the message payload. It can be of any type but defaults to Schema object.
    /// TODO: add any type, or not...
    /// </summary>
    [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
    public JsonSchema? Payload { get; set; }

    /// <summary>
    /// Definition of the correlation ID used for message tracing or matching.
    /// </summary>
    [JsonProperty("correlationId", NullValueHandling = NullValueHandling.Ignore)]
    public ICorrelationId? CorrelationId { get; set; }

    /// <summary>
    /// A string containing the name of the schema format used to define the message payload.
    /// If omitted, implementations should parse the payload as a Schema object.
    /// </summary>
    [JsonProperty("schemaFormat", NullValueHandling = NullValueHandling.Ignore)]
    public string? SchemaFormat { get; set; }

    /// <summary>
    /// The content type to use when encoding/decoding a message’s payload.
    /// The value MUST be a specific media type (e.g. application/json).
    /// </summary>
    [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
    public string? ContentType { get; set; }

    /// <summary>
    /// A machine-friendly name for the message.
    /// </summary>
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; }

    /// <summary>
    /// A human-friendly title for the message.
    /// </summary>
    [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
    public string? Title { get; set; }

    /// <summary>
    /// A short summary of what the message is about.
    /// </summary>
    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string? Summary { get; set; }

    /// <summary>
    /// A verbose explanation of the message. CommonMark syntax can be used for rich text representation.
    /// </summary>
    [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
    public string? Description { get; set; }

    /// <summary>
    /// A list of tags for API documentation control. Tags can be used for logical grouping of messages.
    /// </summary>
    [JsonProperty("tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public HashSet<Tag>? Tags { get; set; }

    /// <summary>
    /// Additional external documentation for this message.
    /// </summary>
    [JsonProperty("externalDocs", NullValueHandling = NullValueHandling.Ignore)]
    public ExternalDocumentation? ExternalDocs { get; set; }

    /// <summary>
    /// A free-form map where the keys describe the name of the protocol and the values describe protocol-specific definitions for the message.
    /// </summary>
    [JsonProperty("bindings", NullValueHandling = NullValueHandling.Ignore)]
    public IMessageBindings? Bindings { get; set; }

    /// <summary>
    /// An array with examples of valid message objects.
    /// </summary>
    [JsonProperty("examples")]
    public List<MessageExample> Examples { get; set; } = [];

    /// <summary>
    /// A list of traits to apply to the message object.
    /// Traits MUST be merged into the message object using the JSON Merge Patch algorithm in the same order they are defined here.
    /// The resulting object MUST be a valid Message Object.
    /// </summary>
    [JsonProperty("traits")]
    public List<IMessageTrait> Traits { get; set; } = [];
}

public class MessageExample
{
    /// <summary>
    /// A machine friendly name for the example.
    /// </summary>
    [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    public string? Name { get; set; }

    /// <summary>
    /// A short summary of what the example is about.
    /// </summary>
    [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
    public string? Summary { get; set; }

    /// <summary>
    /// Example of headers that will be included in the message.
    /// </summary>
    [JsonProperty("headers", NullValueHandling = NullValueHandling.Ignore)]
    public Dictionary<string, object>? Headers { get; set; }

    /// <summary>
    /// Example message payload.
    /// </summary>
    [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
    public object? Payload { get; set; }
}