using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using LEGO.AsyncAPI.Models;
using Saunter.AttributeProvider.Attributes;
using Saunter.Options.Filters;

namespace Saunter.Options
{
    public class AsyncApiOptions
    {
        private readonly List<Type> _documentFilters = new();
        private readonly List<Type> _channelFilters = new();
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
        /// Selects the types that will be used to generate an AsyncAPI document.
        /// The second argument is the requested document name, or <see langword="null"/>
        /// for the default document.
        /// </summary>
        /// <remarks>
        /// By default, types are selected using the <see cref="AsyncApiAttribute"/> and
        /// its document name. Replace this predicate to select types without applying
        /// <see cref="AsyncApiAttribute"/> to application classes.
        /// </remarks>
        public Func<Type, string?, bool> AsyncApiTypeSelector { get; set; } =
            (type, documentName) => type.GetCustomAttribute<AsyncApiAttribute>()?.DocumentName == documentName;

        internal virtual IReadOnlyCollection<TypeInfo> AsyncApiSchemaTypes => AssemblyMarkerTypes
            .Select(t => t.Assembly)
            .Distinct()
            .SelectMany(a => a.DefinedTypes)
            .ToImmutableHashSet();

        /// <summary>
        /// A list of filters that will be applied to the generated AsyncAPI document.
        /// </summary>
        public IEnumerable<Type> DocumentFilters => _documentFilters;

        /// <summary>
        /// A list of filters that will be applied to any generated channels.
        /// </summary>
        public IEnumerable<Type> ChannelFilters => _channelFilters;

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
        public void AddAsyncApiChannelFilter<T>() where T : IChannelFilter
        {
            _channelFilters.Add(typeof(T));
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

        public ConcurrentDictionary<string, AsyncApiDocument> NamedApis { get; set; } = new();
    }
}
