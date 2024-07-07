// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using LEGO.AsyncAPI.Readers;
using Microsoft.Extensions.Options;
using Saunter.Serialization;
using Saunter;
using System.Runtime.Loader;
using System.Reflection;
using LEGO.AsyncAPI;
using LEGO.AsyncAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using AsyncApi.Saunter.Generator.Cli.SwashbuckleImport;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Saunter.AsyncApiSchema.v2;
using static Program;
using AsyncApiDocument = Saunter.AsyncApiSchema.v2.AsyncApiDocument;
using System.IO;

namespace AsyncApi.Saunter.Generator.Cli.Commands;

internal class TofileInternal
{
    private const string defaultDocumentName = null;

    internal static int Run(IDictionary<string, string> namedArgs)
    {
        // 1) Configure host with provided startupassembly
        var startupAssembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(Path.Combine(Directory.GetCurrentDirectory(), namedArgs[StartupAssemblyArgument]));

        // 2) Build a service container that's based on the startup assembly
        var envVars = (namedArgs.TryGetValue(EnvOption, out var x) && !string.IsNullOrWhiteSpace(x)) ? x.Split(',').Select(x => x.Trim()) : Array.Empty<string>();
        foreach (var envVar in envVars.Select(x => x.Split('=').Select(x => x.Trim()).ToList()))
        {
            if (envVar.Count == 2)
            {
                Environment.SetEnvironmentVariable(envVar[0], envVar[1], EnvironmentVariableTarget.Process);
            }
            else
            {
                throw new ArgumentOutOfRangeException(EnvOption, namedArgs[EnvOption], "Environment variable should be in the format: env1=value1,env2=value2");
            }
        }
        var serviceProvider = GetServiceProvider(startupAssembly);

        // 3) Retrieve AsyncAPI via configured provider
        var documentProvider = serviceProvider.GetService<IAsyncApiDocumentProvider>();
        var asyncapiOptions = serviceProvider.GetService<IOptions<AsyncApiOptions>>().Value;
        var documentSerializer = serviceProvider.GetRequiredService<IAsyncApiDocumentSerializer>();

        var documentNames = (namedArgs.TryGetValue(DocOption, out var doc) && !string.IsNullOrWhiteSpace(doc)) ? [doc] : asyncapiOptions.NamedApis.Keys;
        var fileTemplate = (namedArgs.TryGetValue(FileNameOption, out var template) && !string.IsNullOrWhiteSpace(template)) ? template : "{document}_asyncapi.{extension}";
       if (documentNames.Count == 0)
        {
            if (asyncapiOptions.AssemblyMarkerTypes.Any())
            {
                documentNames = [defaultDocumentName];
            }
            else
            {
                throw new ArgumentOutOfRangeException(DocOption, $"No AsyncAPI documents found: {DocOption} = '{doc}'. Known document(s): {string.Join(", ", asyncapiOptions.NamedApis.Keys)}.");
            }
        }

        foreach (var documentName in documentNames)
        {
            AsyncApiDocument prototype;
            if (documentName == defaultDocumentName)
            {
                prototype = asyncapiOptions.AsyncApi;
            }
            else if (!asyncapiOptions.NamedApis.TryGetValue(documentName, out prototype))
            {
                throw new ArgumentOutOfRangeException(DocOption, documentName, $"Requested AsyncAPI document not found: '{documentName}'. Known document(s): {string.Join(", ", asyncapiOptions.NamedApis.Keys)}.");
            }

            var schema = documentProvider.GetDocument(asyncapiOptions, prototype);
            var asyncApiSchemaJson = documentSerializer.Serialize(schema);
            var asyncApiDocument = new AsyncApiStringReader().Read(asyncApiSchemaJson, out var diagnostic);
            if (diagnostic.Errors.Any())
            {
                Console.Error.WriteLine($"AsyncAPI Schema '{documentName ?? "default"}' is not valid ({diagnostic.Errors.Count} Error(s), {diagnostic.Warnings.Count} Warning(s)):" +
                                        $"{Environment.NewLine}{string.Join(Environment.NewLine, diagnostic.Errors.Select(x => $"- {x}"))}");
            }

            // 4) Serialize to specified output location or stdout
            var outputPath = (namedArgs.TryGetValue(OutputOption, out var path) && !string.IsNullOrWhiteSpace(path)) ? Path.Combine(Directory.GetCurrentDirectory(), path) : null;
            if (!string.IsNullOrEmpty(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var exportJson = true;
            var exportYml = false;
            var exportYaml = false;
            if (namedArgs.TryGetValue(FormatOption, out var format) && !string.IsNullOrWhiteSpace(format))
            {
                var splitted = format.Split(',').Select(x => x.Trim()).ToList();
                exportJson = splitted.Any(x => x.Equals("json", StringComparison.OrdinalIgnoreCase));
                exportYml = splitted.Any(x => x.Equals("yml", StringComparison.OrdinalIgnoreCase));
                exportYaml = splitted.Any(x => x.Equals("yaml", StringComparison.OrdinalIgnoreCase));
            }

            if (exportJson)
            {
                WriteFile(AddFileExtension(outputPath, fileTemplate, documentName, "json"), stream => asyncApiDocument.SerializeAsJson(stream, AsyncApiVersion.AsyncApi2_0));
            }

            if (exportYml)
            {
                WriteFile(AddFileExtension(outputPath, fileTemplate, documentName, "yml"), stream => asyncApiDocument.SerializeAsYaml(stream, AsyncApiVersion.AsyncApi2_0));
            }

            if (exportYaml)
            {
                WriteFile(AddFileExtension(outputPath, fileTemplate, documentName, "yaml"), stream => asyncApiDocument.SerializeAsYaml(stream, AsyncApiVersion.AsyncApi2_0));
            }
        }

        return 0;
    }

    private static void WriteFile(string outputPath, Action<Stream> writeAction)
    {
        using var stream = outputPath != null ? File.Create(outputPath) : Console.OpenStandardOutput();
        writeAction(stream);

        if (outputPath != null)
        {
            Console.WriteLine($"AsyncAPI {Path.GetExtension(outputPath)[1..]} successfully written to {outputPath}");
        }
    }

    private static string AddFileExtension(string outputPath, string fileTemplate, string documentName, string extension)
    {
        if (outputPath == null)
        {
            return outputPath;
        }

        return Path.Combine(outputPath, fileTemplate.Replace("{document}", documentName == defaultDocumentName ? "" : documentName).Replace("{extension}", extension).TrimStart('_'));
    }

    private static IServiceProvider GetServiceProvider(Assembly startupAssembly)
    {
        if (TryGetCustomHost(startupAssembly, "AsyncAPIHostFactory", "CreateHost", out IHost host))
        {
            return host.Services;
        }

        if (TryGetCustomHost(startupAssembly, "AsyncAPIWebHostFactory", "CreateWebHost", out IWebHost webHost))
        {
            return webHost.Services;
        }

        try
        {
            return WebHost.CreateDefaultBuilder().UseStartup(startupAssembly.GetName().Name).Build().Services;
        }
        catch
        {
            var serviceProvider = HostingApplication.GetServiceProvider(startupAssembly);

            if (serviceProvider != null)
            {
                return serviceProvider;
            }

            throw;
        }
    }

    private static bool TryGetCustomHost<THost>(Assembly startupAssembly, string factoryClassName, string factoryMethodName, out THost host)
    {
        // Scan the assembly for any types that match the provided naming convention
        var factoryTypes = startupAssembly.DefinedTypes.Where(t => t.Name == factoryClassName).ToList();

        if (factoryTypes.Count == 0)
        {
            host = default;
            return false;
        }
        else if (factoryTypes.Count > 1)
        {
            throw new InvalidOperationException($"Multiple {factoryClassName} classes detected");
        }

        var factoryMethod = factoryTypes.Single().GetMethod(factoryMethodName, BindingFlags.Public | BindingFlags.Static);

        if (factoryMethod == null || factoryMethod.ReturnType != typeof(THost))
        {
            throw new InvalidOperationException($"{factoryClassName} class detected but does not contain a public static method called {factoryMethodName} with return type {typeof(THost).Name}");
        }

        host = (THost)factoryMethod.Invoke(null, null);
        return true;
    }
}
