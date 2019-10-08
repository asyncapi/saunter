using System;
using System.Collections.Generic;
using Saunter.Generation.Filters;

namespace Saunter.Generation
{
    public class AsyncApiDocumentGeneratorOptions
    {
        /// <summary>
        /// The base asyncapi schema.
        /// This will be augmented with other information auto-discovered from attributes.
        /// </summary>
        public AsyncApiSchema.v2.AsyncApiDocument AsyncApi { get; set; }

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
        
        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IList<IDocumentFilter> DocumentFilters { get; } = new List<IDocumentFilter>();
        
        /// <summary>
        /// A list of filters that will be applied to any generated Publish operations.
        /// </summary>
        public IList<IPublishOperationFilter> PublishOperationFilters { get; } = new List<IPublishOperationFilter>();
    }
}