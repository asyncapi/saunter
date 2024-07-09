using ConsoleAppFramework;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.Logging;

namespace AsyncAPI.Saunter.Generator.Cli.ToFile;

internal class ToFileCommand(ILogger<ToFileCommand> logger, EnvironmentBuilder environment, ServiceProviderBuilder builder, AsyncApiDocumentExtractor docExtractor, FileWriter fileWriter)
{
    private const string DEFAULT_FILENAME = "{document}_asyncapi.{extension}";
    
    /// <summary>
    /// Retrieves AsyncAPI spec from a startup assembly and writes to file.
    /// </summary>
    /// <param name="startupassembly">relative path to the application's startup assembly</param>
    /// <param name="output">-o,relative path where the AsyncAPI will be output [defaults to stdout]</param>
    /// <param name="doc">-d,name(s) of the AsyncAPI documents you want to retrieve, as configured in your startup class [defaults to all documents]</param>
    /// <param name="format">exports AsyncAPI in json and/or yml format [defaults to json]</param>
    /// <param name="filename">defines the file name template, {document} and {extension} template variables can be used [defaults to "{document}_asyncapi.{extension}\"]</param>
    /// <param name="env">define environment variable(s) for the application. Formatted as a comma separated list of key=value pairs or just key for flags</param>
    [Command("tofile")]
    public int ToFile([Argument] string startupassembly, string output = "./", string doc = null, string format = "json", string filename = DEFAULT_FILENAME, string env = "")
    {
        if (!File.Exists(startupassembly))
        {
            throw new FileNotFoundException(startupassembly);
        }

        // Prepare environment
        environment.SetEnvironmentVariables(env);

        // Get service provider from startup assembly
        var serviceProvider = builder.BuildServiceProvider(startupassembly);

        // Retrieve AsyncAPI via service provider
        var documents = docExtractor.GetAsyncApiDocument(serviceProvider, !string.IsNullOrWhiteSpace(doc) ? doc.Split(',', StringSplitOptions.RemoveEmptyEntries) : null);
        foreach (var (documentName, asyncApiDocument) in documents)
        {
            // Serialize to specified output location or stdout
            var outputPath = !string.IsNullOrWhiteSpace(output) ? Path.Combine(Directory.GetCurrentDirectory(), output) : null;
            if (!string.IsNullOrEmpty(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var exportJson = true;
            var exportYml = false;
            var exportYaml = false;
            if (!string.IsNullOrWhiteSpace(format))
            {
                var splitted = format.Split(',').Select(x => x.Trim()).ToList();
                exportJson = splitted.Any(x => x.Equals("json", StringComparison.OrdinalIgnoreCase));
                exportYml = splitted.Any(x => x.Equals("yml", StringComparison.OrdinalIgnoreCase));
                exportYaml = splitted.Any(x => x.Equals("yaml", StringComparison.OrdinalIgnoreCase));
            }
            logger.LogDebug($"Format: exportJson={exportJson}, exportYml={exportYml}, exportYaml={exportYaml}.");

            var fileTemplate = !string.IsNullOrWhiteSpace(filename) ? filename : DEFAULT_FILENAME;
            if (exportJson)
            {
                fileWriter.Write(outputPath, fileTemplate, documentName, "json", stream => asyncApiDocument.SerializeAsJson(stream, AsyncApiVersion.AsyncApi2_0));
            }

            if (exportYml)
            {
                fileWriter.Write(outputPath, fileTemplate, documentName, "yml", stream => asyncApiDocument.SerializeAsYaml(stream, AsyncApiVersion.AsyncApi2_0));
            }

            if (exportYaml)
            {
                fileWriter.Write(outputPath, fileTemplate, documentName, "yaml", stream => asyncApiDocument.SerializeAsYaml(stream, AsyncApiVersion.AsyncApi2_0));
            }
        }

        return 1;
    }
}
