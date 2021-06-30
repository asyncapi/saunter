using System;
using System.Linq;
using System.Reflection;
using Saunter.AsyncApiSchema.v2;
using Saunter.Attributes;
using Saunter.Utils;

namespace Saunter.Generation
{
    public class AsyncApiDocumentProvider : IAsyncApiDocumentProvider
    {
        private readonly IDocumentGenerator _documentGenerator;

        public AsyncApiDocumentProvider(IDocumentGenerator documentGenerator)
        {
            _documentGenerator = documentGenerator;
        }

        public AsyncApiDocument GetDocument(AsyncApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            var asyncApiTypes = GetAsyncApiTypes(options);

            var document = _documentGenerator.GenerateDocument(asyncApiTypes, options);

            return document;
        }
        
        
        /// <summary>
        /// Get all types with an <see cref="AsyncApiAttribute"/> from assemblies contAsyncApiOptionsef="SaunterOptions.AssemblyMarkerTypes"/>.
        /// </summary>
        private TypeInfo[] GetAsyncApiTypes(AsyncApiOptions options)
        {
            var assembliesToScan = options.AssemblyMarkerTypes.Select(t => t.Assembly);

            var asyncApiTypes = assembliesToScan
                .SelectMany(a => a.DefinedTypes.Where(t => t.GetCustomAttribute<AsyncApiAttribute>()?.ApiName == options.ApiName))
                .ToArray();

            return asyncApiTypes;
        }
    }
}