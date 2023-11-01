using System;

namespace Saunter.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class ChannelParameterAttribute : Attribute
{
    public ChannelParameterAttribute(string name, Type type)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }

    public string Name { get; }

    public Type Type { get; }

    public string? Description { get; set; }

    public string? Location { get; set; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public abstract class OperationAttribute : Attribute
{
    protected OperationAttribute(string channelName)
    {
        ChannelName = channelName;
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

    public OperationType OperationType { get; protected set; }

    public Type? MessagePayloadType { get; protected set; }

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
    public string[]? Tags { get; protected set; }
}

public class PublishOperationAttribute<TMessage> : PublishOperationAttribute where TMessage : notnull
{
    public PublishOperationAttribute(string channelName, params string[] tags) : base(channelName)
    {
        OperationType = OperationType.Publish;
        MessagePayloadType = typeof(TMessage);
        Tags = tags;
    }

    public PublishOperationAttribute(string channelName) : base(channelName)
    {
        OperationType = OperationType.Publish;
        MessagePayloadType = typeof(TMessage);
    }
}

public class PublishOperationAttribute : OperationAttribute
{
    public PublishOperationAttribute(string channelName, Type messagePayloadType, params string[] tags) : base(channelName)
    {
        OperationType = OperationType.Publish;
        MessagePayloadType = messagePayloadType;
        Tags = tags;
    }
    public PublishOperationAttribute(string channelName, Type messagePayloadType) : base(channelName)
    {
        OperationType = OperationType.Publish;
        MessagePayloadType = messagePayloadType;
    }

    public PublishOperationAttribute(string channelName) : base(channelName)
    {
        OperationType = OperationType.Publish;
    }
}

public class SubscribeOperationAttribute<TMessage> : SubscribeOperationAttribute where TMessage : notnull
{
    public SubscribeOperationAttribute(string channelName, params string[] tags) : base(channelName)
    {
        OperationType = OperationType.Subscribe;
        MessagePayloadType = typeof(TMessage);
        Tags = tags;
    }

    public SubscribeOperationAttribute(string channelName) : base(channelName)
    {
        OperationType = OperationType.Subscribe;
        MessagePayloadType = typeof(TMessage);
    }
}

public class SubscribeOperationAttribute : OperationAttribute
{
    public SubscribeOperationAttribute(string channelName, Type messagePayloadType, params string[] tags) : base(channelName)
    {
        OperationType = OperationType.Subscribe;
        MessagePayloadType = messagePayloadType;
        Tags = tags;
    }

    public SubscribeOperationAttribute(string channelName, Type messagePayloadType) : base(channelName)
    {
        OperationType = OperationType.Subscribe;
        MessagePayloadType = messagePayloadType;
    }

    public SubscribeOperationAttribute(string channelName) : base(channelName)
    {
        OperationType = OperationType.Subscribe;
    }
}

public enum OperationType
{
    Publish,
    Subscribe
}
