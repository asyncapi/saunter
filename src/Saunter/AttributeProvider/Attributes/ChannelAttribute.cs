using System;

namespace Saunter.AttributeProvider.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class ChannelAttribute : Attribute
    {
        /// <summary>
        /// The name of the channel. 
        /// Format depends on the underlying messaging protocol's conventions.
        /// For example, amqp uses dot-separated paths 'light.measured'.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// An optional description of this channel item.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The name of a channel bindings item to reference.
        /// The bindings must be added to components/channelBindings with the same name.
        /// </summary>
        public string? BindingsRef { get; set; }

        /// <summary>
        /// The servers on which this channel is available, specified as an optional unordered
        /// list of names (string keys) of Server Objects defined in the Servers Object (a map).
        /// If servers is absent or empty then this channel must be available on all servers
        /// defined in the Servers Object.
        /// </summary>
        public string[] Servers { get; set; }

        /// <summary>
        /// Creates a new channel documentation using the given topic name
        /// </summary>
        /// <param name="name">The topic's name</param>
        /// <exception cref="ArgumentNullException">thrown if the passed topic name is null.</exception>
        public ChannelAttribute(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Servers = Array.Empty<string>();
        }

        /// <summary>
        /// Creates a new channel documentation using the given resolver type
        /// and the given message type, where the resolver must implement IChannelResolver.
        /// </summary>
        /// <param name="resolverType">a resolver of type IChannelResolver</param>
        /// <param name="messageType">the message type</param>
        /// <exception cref="ArgumentException">if the passed resolver type is not implementing IChannelResolver</exception>
        /// <exception cref="ArgumentNullException">if any of the passed types is null</exception>
        public ChannelAttribute(Type resolverType, Type messageType)
        {
            ArgumentNullException.ThrowIfNull(nameof(resolverType));
            ArgumentNullException.ThrowIfNull(nameof(messageType));

            IChannelResolver resolver = Activator.CreateInstance(resolverType, messageType) as IChannelResolver
                ?? throw new ArgumentException("resolverType must implement IChannelResolver", nameof(resolverType));
            Name = resolver.ResolveChannelName() ?? throw new ArgumentNullException(nameof(Name));
            Servers = Array.Empty<string>();
        }
    }
}
