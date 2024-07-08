using System;

namespace Saunter.AttributeProvider.Attributes
{
    /// <summary>
    /// Marks a class as containing asyncapi channels.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AsyncApiAttribute : Attribute
    {
        public string DocumentName { get; }

        public AsyncApiAttribute(string documentName = null)
        {
            DocumentName = documentName;
        }
    }
}
