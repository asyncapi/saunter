using System;
using System.Reflection;

namespace AsyncApi.Net.Generator.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public abstract class OperationAttribute : Attribute
{
    protected OperationAttribute(string channelName, string[] tags, TypeInfo[] payloadTypes)
    {
        ChannelName = channelName;
        MessagePayloadTypes = payloadTypes;
        Tags = tags;
    }

    /// <summary>
    /// The name of the channel.
    /// Format depends on the underlying messaging protocol's conventions.
    /// For example, amqp uses dot-separated paths 'light.measured'.
    /// </summary>
    public string ChannelName { get; }

    /// <summary>
    /// An optional description of this channel item.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? ChannelDescription { get; set; }

    /// <summary>
    /// The name of a channel bindings item to reference.
    /// The bindings must be added to components/channelBindings with the same name.
    /// </summary>
    public string? ChannelBindingsRef { get; set; }

    /// <summary>
    /// The servers on which this channel is available, specified as an optional unordered
    /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
    /// If servers is absent or empty then this channel must be available on all servers
    /// defined in the Servers Object.
    /// </summary>
    public string[]? ChannelServers { get; set; }

    /// <summary>
    /// Operation Type (Sub/Pub)
    /// </summary>
    public OperationType OperationType { get; protected set; }

    /// <summary>
    /// Message schema mark
    /// Id for match with message attribute
    /// </summary>
    public TypeInfo[] MessagePayloadTypes { get; protected set; }

    /// <summary>
    /// A short summary of what the operation is about.
    /// </summary>
    public string? Summary { get; set; }

    /// <summary>
    /// Unique string used to identify the operation.
    /// The id MUST be unique among all operations described in the API.
    /// The operationId value is case-sensitive.
    /// Tools and libraries MAY use the operationId to uniquely identify an operation,
    /// therefore, it is RECOMMENDED to follow common programming naming conventions.
    /// </summary>
    public string? OperationId { get; set; }

    /// <summary>
    /// A verbose explanation of the operation.
    /// CommonMark syntax can be used for rich text representation.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The name of an operation bindings item to reference.
    /// The bindings must be added to components/operationBindings with the same name.
    /// </summary>
    public string? BindingsRef { get; set; }

    /// <summary>
    /// A list of tags for API documentation control. Tags can be used for logical grouping of operations.
    /// </summary>
    public string[] Tags { get; protected set; }

    /// <summary>
    /// Name asyncapi document
    /// </summary>
    public string? DocumentName { get; set; }
}

public class PublishOperationAttribute : OperationAttribute
{
    public PublishOperationAttribute(string channelName, TypeInfo messagePayloadType, params string[] tags) :
        base(channelName, tags, new TypeInfo[] { messagePayloadType })
    {
        OperationType = OperationType.Publish;
    }

    public PublishOperationAttribute(string channelName, TypeInfo[] messagePayloadTypes, params string[] tags) :
        base(channelName, tags, messagePayloadTypes)
    {
        OperationType = OperationType.Publish;
    }
}

public class SubscribeOperationAttribute : OperationAttribute
{
    public SubscribeOperationAttribute(string channelName, TypeInfo messagePayloadType, params string[] tags) :
        base(channelName, tags, new TypeInfo[] { messagePayloadType })
    {
        OperationType = OperationType.Subscribe;
    }

    public SubscribeOperationAttribute(string channelName, TypeInfo[] messagePayloadTypes, params string[] tags) :
        base(channelName, tags, messagePayloadTypes)
    {
        OperationType = OperationType.Subscribe;
    }
}

public class PublishOperationAttribute<TMessage> : PublishOperationAttribute where TMessage : notnull
{
    public PublishOperationAttribute(string channelName, params string[] tags) :
        base(channelName, (TypeInfo)typeof(TMessage), tags)
    { }
}

public class SubscribeOperationAttribute<TMessage> : SubscribeOperationAttribute where TMessage : notnull
{
    public SubscribeOperationAttribute(string channelName, params string[] tags) :
        base(channelName, (TypeInfo)typeof(TMessage), tags)
    { }
}

public class PublishOperationAttribute<TFirstMessage, TSecondMessage> : PublishOperationAttribute
    where TFirstMessage : notnull
    where TSecondMessage : notnull
{
    public PublishOperationAttribute(string channelName, params string[] tags) :
        base(channelName, new TypeInfo[] { (TypeInfo)typeof(TFirstMessage), (TypeInfo)typeof(TSecondMessage) }, tags)
    { }
}

public class SubscribeOperationAttribute<TFirstMessage, TSecondMessage> : SubscribeOperationAttribute
    where TFirstMessage : notnull
    where TSecondMessage : notnull
{
    public SubscribeOperationAttribute(string channelName, params string[] tags) :
        base(channelName, new TypeInfo[] { (TypeInfo)typeof(TFirstMessage), (TypeInfo)typeof(TSecondMessage) }, tags)
    { }
}

public class PublishOperationAttribute<TFirstMessage, TSecondMessage, TThirdMessage> : PublishOperationAttribute
    where TFirstMessage : notnull
    where TSecondMessage : notnull
    where TThirdMessage : notnull
{
    public PublishOperationAttribute(string channelName, params string[] tags) :
        base(channelName, new TypeInfo[] { (TypeInfo)typeof(TFirstMessage), (TypeInfo)typeof(TSecondMessage), (TypeInfo)typeof(TThirdMessage) }, tags)
    { }
}

public class SubscribeOperationAttribute<TFirstMessage, TSecondMessage, TThirdMessage> : SubscribeOperationAttribute
    where TFirstMessage : notnull
    where TSecondMessage : notnull
    where TThirdMessage : notnull
{
    public SubscribeOperationAttribute(string channelName, params string[] tags) :
        base(channelName, new TypeInfo[] { (TypeInfo)typeof(TFirstMessage), (TypeInfo)typeof(TSecondMessage), (TypeInfo)typeof(TThirdMessage) }, tags)
    { }
}

public enum OperationType
{
    Publish,
    Subscribe
}
