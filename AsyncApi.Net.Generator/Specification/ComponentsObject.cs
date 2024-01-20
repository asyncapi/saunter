using System.Collections.Generic;

namespace AsyncApi.Net.Generator.Specification;

/// <summary>
/// Object holding a set of reusable objects for different aspects of the AsyncAPI specification.
/// All objects defined within the components object will have no effect on the API unless they are explicitly referenced from properties outside the components object.
/// </summary>
public class ComponentsObject
{
    /// <summary>
    /// Gets or sets an object to hold reusable Schema Objects. If this is a Schema Object, then the schemaFormat will be assumed to be "application/vnd.aai.asyncapi+json;version=asyncapi" where the version is equal to the AsyncAPI Version String.
    /// </summary>
    public Dictionary<string, MultiFormatSchemaObject> Schemas { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Server Objects.
    /// </summary>
    public Dictionary<string, ServerObject> Servers { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Channel Objects.
    /// </summary>
    public Dictionary<string, ChannelObject> Channels { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Operation Objects.
    /// </summary>
    public Dictionary<string, OperationObject> Operations { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Message Objects.
    /// </summary>
    public Dictionary<string, MessageObject> Messages { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Security Scheme Objects.
    /// </summary>
    public Dictionary<string, SecuritySchemeObject> SecuritySchemes { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Server Variable Objects.
    /// </summary>
    public Dictionary<string, ServerVariableObject> ServerVariables { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Parameter Objects.
    /// </summary>
    public Dictionary<string, ParameterObject> Parameters { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Correlation ID Objects.
    /// </summary>
    public Dictionary<string, CorrelationIdObject> CorrelationIds { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Operation Reply Objects.
    /// </summary>
    public Dictionary<string, OperationReplyObject> Replies { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Operation Reply Address Objects.
    /// </summary>
    public Dictionary<string, OperationReplyAddressObject> ReplyAddresses { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable External Documentation Objects.
    /// </summary>
    public Dictionary<string, ExternalDocumentationObject> ExternalDocs { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Tag Objects.
    /// </summary>
    public Dictionary<string, TagObject> Tags { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Operation Trait Objects.
    /// </summary>
    public Dictionary<string, OperationTraitObject> OperationTraits { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Message Trait Objects.
    /// </summary>
    public Dictionary<string, MessageTraitObject> MessageTraits { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Server Bindings Objects.
    /// </summary>
    public Dictionary<string, ServerBindingsObject> ServerBindings { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Channel Bindings Objects.
    /// </summary>
    public Dictionary<string, ChannelBindingsObject> ChannelBindings { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Operation Bindings Objects.
    /// </summary>
    public Dictionary<string, OperationBindingsObject> OperationBindings { get; } = new();

    /// <summary>
    /// Gets or sets an object to hold reusable Message Bindings Objects.
    /// </summary>
    public Dictionary<string, MessageBindingsObject> MessageBindings { get; } = new();
}
