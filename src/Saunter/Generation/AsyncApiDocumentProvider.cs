using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Options;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Utils;

namespace Saunter.Generation
{
    public class AsyncApiDocumentProvider : IAsyncApiDocumentProvider
    {
        private readonly IDocumentGenerator _documentGenerator;
        private readonly AsyncApiOptions _options;

        public AsyncApiDocumentProvider(IOptions<AsyncApiOptions> options, IDocumentGenerator documentGenerator)
        {
            _documentGenerator = documentGenerator;
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        
        public AsyncApiDocument GetDocument()
        {
            var asyncApiTypes = GetAsyncApiTypes();

            var document = _documentGenerator.GenerateDocument(asyncApiTypes);

            return document;
        }
        
        
        /// <summary>
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies contAsyncApiOptionsef="SaunterOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private TypeInfo[] GetAsyncApiTypes()
        {
            var assembliesToScan = _options.AssemblyMarkerTypes.Select(t => t.Assembly);

            var asyncApiTypes = assembliesToScan
                .SelectMany(a => a.DefinedTypes.Where(t => t.HasCustomAttribute<AsyncApiAttribute>()))
                .ToArray();

            return asyncApiTypes;
        }
    }
}