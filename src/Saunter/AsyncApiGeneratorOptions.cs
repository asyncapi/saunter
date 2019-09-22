using System;
using System.Collections.Generic;

namespace Saunter
{
    public class AsyncApiGeneratorOptions
    {
        /// <summary>
        /// The base asyncapi schema.
        /// This will be augmented with other information auto-discovered from attributes.
        /// </summary>
        public AsyncApiSchema.v2_0_0.AsyncApiSchema AsyncApiSchema { get; set; }

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
    }
}
