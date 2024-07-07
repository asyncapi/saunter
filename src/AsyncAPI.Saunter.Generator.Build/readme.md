# AsyncApi Generator.Build Nuget Package
A nuget package to generate AsyncAPI specification files at build time, based on code-first attributes.

# Customizations
The AsyncAPI spec generation can be configured through project properties in the csproj-file (or .props files):
```
  <PropertyGroup>
    <AsyncAPIGenerateDocumentsOnBuild></AsyncAPIGenerateDocumentsOnBuild>
    <AsyncAPIDocumentFormats></AsyncAPIDocumentFormats>
    <AsyncAPIDocumentOutputPath></AsyncAPIDocumentOutputPath>
    <AsyncAPIDocumentNames></AsyncAPIDocumentNames>
    <AsyncAPIDocumentFilename></AsyncAPIDocumentFilename>
    <AsyncAPIDocumentEnvVars></AsyncAPIDocumentEnvVars>
  </PropertyGroup>
```

Defaults are the same as the underlying [Generator.Cli tool](https://www.nuget.org/packages/AsyncAPI.Saunter.Generator.Cli).  

If the ```AsyncAPI.Saunter.Generator.Build``` Nuget package is referenced, the default is to generate AsyncAPI spec files at build time.

- AsyncAPIGenerateDocumentsOnBuild: Whether to actually generate AsyncAPI spec files on build (true or false, default: true)
- AsyncAPIDocumentFormats: Format of the expected AsyncAPI spec files (json, yml or yaml, default: json)
- AsyncAPIDocumentOutputPath: Output path for the AsyncAPI spec files, relative to the csproj location. (default is the csproj root path: ./)
- AsyncAPIDocumentNames: The AsyncAPI documents to generate. (default: generate all known documents)
- AsyncAPIDocumentFilename: Template of the AsyncAPI spec files (default: "{document}_asyncapi.{extension}")
- AsyncAPIDocumentEnvVars: Environment variable(s) to set during generation of the AsyncAPI spec files (default: none, Example: "ASPNETCORE_ENVIRONMENT=Development")  
None of these properties are mandatory. Only referencing the [AsyncAPI.Saunter.Generator.Build](https://www.nuget.org/packages/AsyncAPI.Saunter.Generator.Build) Nuget package will generate a json AsyncAPI spec file for all AsyncAPI documents.