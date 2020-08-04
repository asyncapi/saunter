using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;

namespace Saunter
{
    public class AsyncApiOptions
    {
        /// <summary>
        /// The base asyncapi schema.
        /// This will be augmented with other information auto-discovered from attributes.
        /// </summary>
        public AsyncApiSchema.v2.AsyncApiDocument AsyncApi { get; set; } = new AsyncApiDocument();

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
        
        /// <summary>
        /// A function to select a schemaId for a type.
        /// </summary>
        public Func<Type, string> SchemaIdSelector { get; set; } = DefaultSchemaIdFactory.Generate;

        /// <summary>
        /// A function to select the name for a property.
        /// </summary>
        public Func<PropertyInfo, string> PropertyNameSelector { get; set; } = prop => new CamelCaseNamingStrategy().GetPropertyName(prop.Name, false); 
        
        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IList<IDocumentFilter> DocumentFilters { get; } = new List<IDocumentFilter>();

        /// <summary>
        /// A list of filters that will be applies to any generated channels.
        /// </summary>
        public IList<IChannelItemFilter> ChannelItemFilters { get; } = new List<IChannelItemFilter>();
        
        /// <summary>
        /// A list of filters that will be applied to any generated Publish operations.
        /// </summary>
        public IList<OperationFilter> OperationFilters { get; } = new List<OperationFilter>();

        /// <summary>
        /// Options related to the Saunter middleware
        /// </summary>
        public AsyncApiMiddlewareOptions Middleware { get; set; } = new AsyncApiMiddlewareOptions();
    }

    public class AsyncApiMiddlewareOptions
    {
        /// <summary>
        /// The route which the AsyncApi document will be hosted
        /// </summary>
        public string Route { get; set; } = "/asyncapi/asyncapi.json";
    }
}