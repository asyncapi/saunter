using System.Reflection;
using System.Runtime.Loader;

using Saunter.AsyncApiSchema.v2;
using Saunter.Serialization;

namespace Saunter.Cli;

/// <summary>
/// Generate documentation from dll
/// </summary>
[Command("specification", "Generate documentation from dll")]
public class SpecificationGeneratorCommand : ConsoleAppBase
{
    private readonly IAsyncApiDocumentProvider _provider;
    private readonly IAsyncApiDocumentSerializer _serializer;

    /// <summary>
    /// Documentation generator from dll
    /// </summary>
    public SpecificationGeneratorCommand(IAsyncApiDocumentProvider provider, IAsyncApiDocumentSerializer serializer)
    {
        _provider = provider;
        _serializer = serializer;
    }

    /// <summary>
    /// Generate specification from dll directory
    /// </summary>
    /// <param name="dllDirectory"></param>
    /// <param name="prototype"></param>
    /// <returns></returns>
    [Command("generate", "Generate specification from dll directory")]
    public async Task Generate([Option("p", "The path containing the target application's dll")] string dllDirectory, AsyncApiPrototype prototype)
    {
        AsyncApiDocument documentPrototype = new()
        {
            Id = prototype.Id,
        };

        if (!string.IsNullOrEmpty(prototype.DefaultContentType))
        {
            documentPrototype.DefaultContentType = prototype.DefaultContentType;
        }

        if (prototype.Info is not null)
        {
            documentPrototype.Info = prototype.Info;
        }

        if (prototype.Servers is not null)
        {
            documentPrototype.Servers = prototype.Servers;
        }

        if (prototype.ExternalDocs is not null)
        {
            documentPrototype.ExternalDocs = prototype.ExternalDocs;
        }

        AsyncApiOptions options = new()
        {
            AsyncApi = documentPrototype,
        };

        string[] dllFiles = Directory.GetFiles(dllDirectory, "*.dll");

        foreach (string dllFile in dllFiles)
        {
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(dllFile);

            options.Assemblies.Add(assembly);
        }

        AsyncApiDocument document = _provider.GetDocument(options, options.AsyncApi);

        string jsonSpecification = _serializer.Serialize(document);

        await using StreamWriter writer = File.CreateText("asyncapi.json");

        await writer.WriteAsync(jsonSpecification);
    }
}
