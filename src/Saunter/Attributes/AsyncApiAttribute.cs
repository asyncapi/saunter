using System;

namespace Saunter.Attributes
{
    /// <summary>
    /// Marks a class as containing asyncapi channels.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AsyncApiAttribute : Attribute
    {
        // Just a marker.   
    }
}