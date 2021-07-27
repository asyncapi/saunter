using System;

namespace Saunter.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class MessageAttribute : Attribute
    {
        public MessageAttribute(Type payloadType)
        {
            PayloadType = payloadType;
        }

        /// <summary>
        /// The type to use to generate the message payload schema.
        /// </summary>
        public Type PayloadType { get; }

        /// <summary>
        /// A machine-friendly name for the message.
        /// Defaults to the generated schemaId.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A human-friendly title for the message.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A short summary of what the message is about.
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// A verbose explanation of the message.
        /// CommonMark syntax can be used for rich text representation.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of a message bindings item to reference.
        /// The bindings must be added to components/messageBindings with the same name.
        /// </summary>
        public string BindingsRef { get; set; }
    }
}