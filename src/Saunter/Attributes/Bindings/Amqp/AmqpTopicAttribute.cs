using System;

namespace Saunter.Attributes.Bindings.Amqp
{
    /// <summary>
    /// Marks a class as producing messages which will be published to an AMQP topic.
    /// This will add an AMQP binding to each channel.produces.message in the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AmqpTopicAttribute : Attribute
    {
        public string Topic { get; }

        /// <inheritdoc cref="AmqpTopicAttribute"/>
        /// <param name="topic">The name of the AMQP topic that messages will be published to</param>
        public AmqpTopicAttribute(string topic)
        {
            Topic = topic ?? throw new ArgumentNullException(nameof(topic));
        }
    }
}