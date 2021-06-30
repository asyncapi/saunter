using System;

namespace Saunter.Attributes
{
    /// <summary>
    /// Marks a class as containing asyncapi channels.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AsyncApiAttribute : Attribute
    {
        public string ApiName { get; }

        // Just a marker.
        public AsyncApiAttribute(string apiName = null)
        {
            ApiName = apiName;
        }
    }
}