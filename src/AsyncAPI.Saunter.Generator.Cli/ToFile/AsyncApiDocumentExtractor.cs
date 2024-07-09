using LEGO.AsyncAPI.Models;
using LEGO.AsyncAPI.Readers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Saunter.Serialization;
using Saunter;
using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal class AsyncApiDocumentExtractor(ILogger<AsyncApiDocumentExtractor> logger)
{
    private IEnumerable<string> GetDocumentNames(string[] requestedDocuments, AsyncApiOptions asyncApiOptions)
    {
        var documentNames = requestedDocuments ?? asyncApiOptions.NamedApis.Keys;
        if (documentNames.Count == 0)
        {
            if (asyncApiOptions.AssemblyMarkerTypes.Any())
            {
                documentNames = [null]; // null marks the default, unnamed, document
            }
            else
            {
                logger.LogCritical($"AsyncAPI documents found. Known named document(s): {string.Join(", ", asyncApiOptions.NamedApis.Keys)}.");
            }
        }
        return documentNames;
    }

    public IEnumerable<(string name, AsyncApiDocument document)> GetAsyncApiDocument(IServiceProvider serviceProvider, string[] requestedDocuments)
    {
        var documentProvider = serviceProvider.GetService<IAsyncApiDocumentProvider>();
        var asyncApiOptions = serviceProvider.GetService<IOptions<AsyncApiOptions>>().Value;
        var documentSerializer = serviceProvider.GetRequiredService<IAsyncApiDocumentSerializer>();

        foreach (var documentName in GetDocumentNames(requestedDocuments, asyncApiOptions))
        {
            if (documentName == null || !asyncApiOptions.NamedApis.TryGetValue(documentName, out var prototype))
            {
                prototype = asyncApiOptions.AsyncApi;
            }

            var schema = documentProvider.GetDocument(asyncApiOptions, prototype);
            var asyncApiSchemaJson = documentSerializer.Serialize(schema);
            var asyncApiDocument = new AsyncApiStringReader().Read(asyncApiSchemaJson, out var diagnostic);
            if (diagnostic.Errors.Any())
            {
                logger.LogError($"AsyncAPI Schema '{documentName ?? "default"}' is not valid ({diagnostic.Errors.Count} Error(s))");
                foreach (var error in diagnostic.Errors)
                {
                    logger.LogError($"- {error}");
                }
            }
            if (diagnostic.Warnings.Any())
            {
                logger.LogWarning($"AsyncAPI Schema '{documentName ?? "default"}' has {diagnostic.Warnings.Count} Warning(s):");
                foreach (var warning in diagnostic.Warnings)
                {
                    logger.LogWarning($"- {warning}");
                }
            }

            yield return (documentName, asyncApiDocument);
        }
    }
}

