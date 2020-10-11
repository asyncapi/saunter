using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Saunter.AsyncApiSchema.v2;
using Saunter.Generation;
using Saunter.Generation.Filters;
using Saunter.Utils;

namespace Saunter
{
    public class AsyncApiOptions
    {
        /// <summary>
        /// The base asyncapi schema.
        /// This will be augmented with other information auto-discovered from attributes.
        /// </summary>
        public AsyncApiDocument AsyncApi { get; set; } = new AsyncApiDocument();

        /// <summary>
        /// A list of marker types from assemblies to scan for Saunter attributes.
        /// </summary>
        public IList<Type> AssemblyMarkerTypes { get; set; } = new List<Type>();
        
        /// <summary>
        /// A function to select a schemaId for a type.
        /// </summary>
        public Func<Type, string> SchemaIdSelector { get; set; } = DefaultSchemaIdFactory.Generate;

        /// <summary>
        /// A function that specifies if the member name of the enum should be used instead of its value.
        /// </summary>
        public Func<Type, bool> UseEnumMemberName { get; set; } = type =>
        {
            var jsonConverterAttribute = type.GetCustomAttribute<JsonConverterAttribute>();
            return jsonConverterAttribute?.ConverterType == typeof(JsonStringEnumConverter)
                || jsonConverterAttribute?.ConverterType?.FullName == "System.Text.Json.Serialization.JsonStringEnumMemberConverter"
                || jsonConverterAttribute?.ConverterType == typeof(EnumMemberConverter);
        };

        /// <summary>
        /// A function that returns the enum member name.
        /// </summary>
        public Func<Type, Enum, string> EnumMemberNameSelector { get; set; } = (type, val) =>
        {
            var converterType = type.GetCustomAttribute<JsonConverterAttribute>()?.ConverterType;
            if (converterType?.FullName == "System.Text.Json.Serialization.JsonStringEnumMemberConverter"
                || converterType == typeof(EnumMemberConverter))
            {
                var enumMemberAttribute = val.GetCustomAttribute<EnumMemberAttribute>();
                if (enumMemberAttribute?.Value != null)
                {
                    return enumMemberAttribute.Value;
                }
            }

            return val.ToString();
        };
        
        /// <summary>
        /// A function to select the name for a property.
        /// </summary>
        public Func<MemberInfo, string> PropertyNameSelector { get; set; } = prop =>
        {
            var jsonPropertyAttribute = prop.GetCustomAttribute<JsonPropertyNameAttribute>();
            if (jsonPropertyAttribute?.Name != null)
            {
                return jsonPropertyAttribute.Name;
            }

            return JsonNamingPolicy.CamelCase.ConvertName(prop.Name);
        };
        
        /// <summary>
        /// A function to filter the properties which will be included.
        /// </summary>
        public Func<MemberInfo, bool> PropertyFilter { get; set; } = prop => prop.GetCustomAttribute<JsonIgnoreAttribute>() == null;

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
        public const string AsyncApiMiddlewareDefaultRoute = "/asyncapi/asyncapi.json";

        /// <summary>
        /// The route which the AsyncApi document will be hosted
        /// </summary>
        public string Route { get; set; } = AsyncApiMiddlewareDefaultRoute;
    }
}