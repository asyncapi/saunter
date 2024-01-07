using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using NJsonSchema;
using NJsonSchema.Generation;

using AsyncApi.Net.Generator.AsyncApiSchema.v2;
using AsyncApi.Net.Generator.Generation.Filters;
using AsyncApi.Net.Generator.Generation.SchemaGeneration;

namespace AsyncApi.Net.Generator;

public class AsyncApiOptions
{
    private readonly List<Type> _documentFilters = [];
    private readonly List<Type> _channelItemFilters = [];
    private readonly List<Type> _operationFilters = [];

    /// <summary>
    /// The base asyncapi schema. This will be augmented with other information auto-discovered
    /// from attributes.
    /// </summary>
    public required AsyncApiDocument AsyncApi { get; set; }

    /// <summary>
    /// A list of marker types from assemblies to scan for AsyncApi.Net.Generator attributes.
    /// </summary>
    public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();

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
    /// Options related to the AsyncApi.Net.Generator middleware.
    /// </summary>
    public AsyncApiMiddlewareOptions Middleware { get; } = new ();

    public ConcurrentDictionary<string, AsyncApiDocument> NamedApis { get; set; } =
        new ConcurrentDictionary<string, AsyncApiDocument>();

    /// <summary>
    /// Settings related to the JSON Schema generation.
    /// </summary>
    public AsyncApiSchemaOptions SchemaOptions { get; set; } = new ();
}

public class AsyncApiSchemaOptions : JsonSchemaGeneratorSettings
{
    public AsyncApiSchemaOptions()
    {
        SchemaType = SchemaType.JsonSchema; // AsyncAPI uses json-schema, see https://github.com/tehmantra/saunter/pull/103#issuecomment-893267360
        TypeNameGenerator = new CamelCaseTypeNameGenerator();
        SerializerSettings = new JsonSerializerSettings
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