using System;
using System.Collections.Generic;

namespace Saunter.Generation
{
    public class AsyncApiGeneratorOptions
    {
        /// <summary>
        /// The base asyncapi schema.
        /// This will be augmented with other information auto-discovered from attributes.
        /// </summary>
        public AsyncApiSchema.v2.AsyncApiSchema AsyncApiSchema { get; set; }

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
        
        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI schema.
        /// </summary>
        public IList<ISchemaFilter> SchemaFilters { get; } = new List<ISchemaFilter>();
        
        /// <summary>
        /// A list of filters that will be applied to any generated Publish operations.
        /// </summary>
        public IList<IPublishOperationFilter> PublishOperationFilters { get; } = new List<IPublishOperationFilter>();
    }
}