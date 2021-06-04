using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema.Generation;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiOptions
    {
        public AsyncApiOptions()
        {
            UseEnumMemberName = DefaultUseEnumMemberName;
        }

        /// <summary>
        /// The base asyncapi schema. This will be augmented with other information auto-discovered
        /// from attributes.
        /// </summary>
        public AsyncApiDocument AsyncApi { get; set; } = new AsyncApiDocument();

        /// <summary>
        ///     A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();

        public JsonSerializerSettings JsonSerializerSettings { get; set; } = new JsonSerializerSettings();

        /// <summary>
        /// A function that specifies if the member name of the enum should be used instead of its value.
        /// </summary>
        public Func<Type, bool> UseEnumMemberName { get; set; }

        private bool DefaultUseEnumMemberName(Type t)
        {
            return JsonSerializerSettings.Converters.OfType<StringEnumConverter>().Any() 
                   || t.GetCustomAttributes<JsonConverterAttribute>().Any(c => c.ConverterType == typeof(StringEnumConverter));
        }

        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IList<IDocumentFilter> DocumentFilters { get; } = new List<IDocumentFilter>();

        /// <summary>
        /// A list of filters that will be applied to any generated channels.
        /// </summary>
        public IList<IChannelItemFilter> ChannelItemFilters { get; } = new List<IChannelItemFilter>();

        /// <summary>
        /// A list of filters that will be applied to any generated Publish operations.
        /// </summary>
        public IList<IOperationFilter> OperationFilters { get; } = new List<IOperationFilter>();

        /// <summary>
        /// Options related to the Saunter middleware
        /// </summary>
        public AsyncApiMiddlewareOptions Middleware { get; set; } = new AsyncApiMiddlewareOptions();

        public JsonSchemaGeneratorSettings JsonSchemaGeneratorSettings { get; set; } =
            new JsonSchemaGeneratorSettings();
    }

    public class AsyncApiMiddlewareOptions
    {
        /// <summary>
        /// The route which the AsyncAPI document will be hosted
        /// </summary>
        public string Route { get; set; } = "/asyncapi/asyncapi.json";

        /// <summary>
        /// The route which the AsyncAPI UI will be hosted
        /// </summary>
        public string UiRoute { get; set; } = "/asyncapi/ui/index.html";

        /// <summary>
        /// The base URL for the AsyncAPI UI
        /// </summary>
        public string UiBaseRoute { get; set; } = "/asyncapi/ui/";

        /// <summary>
        /// The address of an AsyncAPI playground which will be used to generate the HTML from the AsyncAPI document.
        /// </summary>
        public string PlaygroundBaseAddress { get; set; } = "https://playground.asyncapi.io/";
    }
}