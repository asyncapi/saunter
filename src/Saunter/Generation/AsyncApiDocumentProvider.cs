using System;
using System.Linq;
using System.Reflection;

using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation;

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
        TypeInfo[] asyncApiTypes = GetAsyncApiTypes(options);

        AsyncApiDocument document = _documentGenerator.GenerateDocument(asyncApiTypes, options, prototype, _serviceProvider);

        return document;
    }

    private static TypeInfo[] GetAsyncApiTypes(AsyncApiOptions options)
    {
        return options.AssemblyMarkerTypes
            .SelectMany(t => t.Assembly.DefinedTypes)
            .Distinct()
            .ToArray();
    }
}