using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NJsonSchema;
using NJsonSchema.NewtonsoftJson.Generation;

using Saunter.AsyncApiSchema.v2;
using Saunter.Generation.Filters;
using Saunter.Generation.SchemaGeneration;

namespace Saunter
{
    public class AsyncApiOptions
    {
        private readonly List<Type> _documentFilters = new();
        private readonly List<Type> _channelItemFilters = new();
        private readonly List<Type> _operationFilters = new();

        /// <summary>
        /// The base asyncapi schema. This will be augmented with other information auto-discovered
        /// from attributes.
        /// </summary>
        public AsyncApiDocument AsyncApi { get; set; } = new AsyncApiDocument();

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();

        /// <summary>
        /// A list of assemblies to scan for Saunter attributes.
        /// </summary>
        public List<Assembly> Assemblies { get; set; } = new List<Assembly>();

        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IEnumerable<Type> DocumentFilters => _documentFilters;

        /// <summary>
        /// A list of filters that will be applied to any generated channels.
        /// </summary>
        public IEnumerable<Type> ChannelItemFilters => _channelItemFilters;

        /// <summary>
        /// A list of filters that will be applied to any generated Publish/Subscribe operations.
        /// </summary>
        public IEnumerable<Type> OperationFilters => _operationFilters;


        /// <summary>
        /// Add a filter to be applied to the generated AsyncAPI document.
        /// </summary>
        public void AddDocumentFilter<T>() where T : IDocumentFilter
        {
            _documentFilters.Add(typeof(T));
        }

        /// <summary>
        /// Add a filter to be applied to any generated channels.
        /// </summary>
        public void AddChannelItemFilter<T>() where T : IChannelItemFilter
        {
            _channelItemFilters.Add(typeof(T));
        }

        /// <summary>
        /// Add a filter to be applied to any generated Publish/Subscribe operations.
        /// </summary>
        public void AddOperationFilter<T>() where T : IOperationFilter
        {
            _operationFilters.Add(typeof(T));
        }


        /// <summary>
        /// Options related to the Saunter middleware.
        /// </summary>
        public AsyncApiMiddlewareOptions Middleware { get; } = new AsyncApiMiddlewareOptions();

        public ConcurrentDictionary<string, AsyncApiDocument> NamedApis { get; set; } =
            new ConcurrentDictionary<string, AsyncApiDocument>();

        /// <summary>
        /// Settings related to the JSON Schema generation.
        /// </summary>
        public AsyncApiSchemaOptions SchemaOptions { get; set; } = new AsyncApiSchemaOptions();
    }

    public class AsyncApiSchemaOptions : NewtonsoftJsonSchemaGeneratorSettings
    {
        public AsyncApiSchemaOptions()
        {
            SchemaType = SchemaType.JsonSchema; // AsyncAPI uses json-schema, see https://github.com/tehmantra/saunter/pull/103#issuecomment-893267360
            TypeNameGenerator = new CamelCaseTypeNameGenerator();
            SerializerSettings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            };
        }
    }

    public class AsyncApiMiddlewareOptions
    {
        /// <summary>
        /// The route which the AsyncAPI document will be hosted
        /// </summary>
        public string Route { get; set; } = "/asyncapi/asyncapi.json";

        /// <summary>
        /// The base URL for the AsyncAPI UI
        /// </summary>
        public string UiBaseRoute { get; set; } = "/asyncapi/ui/";

        /// <summary>
        /// The title of page for AsyncAPI UI
        /// </summary>
        public string UiTitle { get; set; } = "AsyncAPI";
    }
}