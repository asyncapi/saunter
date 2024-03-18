using System;
using System.Linq;
using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;

namespace Saunter.Generation
{
    public class AsyncApiDocumentProvider : IAsyncApiDocumentProvider
    {
        private readonly IDocumentGenerator _documentGenerator;
        private readonly IServiceProvider _serviceProvider;

        public AsyncApiDocumentProvider(IDocumentGenerator documentGenerator, IServiceProvider serviceProvider)
        {
            _documentGenerator = documentGenerator;
            _serviceProvider = serviceProvider;
        }

        public AsyncApiDocument GetDocument(AsyncApiOptions options, AsyncApiDocument prototype)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var asyncApiTypes = GetAsyncApiTypes(options, prototype);

            var document = _documentGenerator.GenerateDocument(asyncApiTypes, options, prototype, _serviceProvider);

            return document;
        }


        /// <summary>
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies <see cref="AsyncApiOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private static TypeInfo[] GetAsyncApiTypes(AsyncApiOptions options, AsyncApiDocument prototype)
        {
            var asyncApiTypes = options
                .AssemblyMarkerTypes
                .Select(t => t.Assembly)
                .Distinct()
                .Union(options.Assemblies)
                .SelectMany(a => a.DefinedTypes
                    .Where(t => t.GetCustomAttribute<AsyncApiAttribute>()?.DocumentName == prototype.DocumentName))
                .ToArray();

            return asyncApiTypes;
        }
    }
}