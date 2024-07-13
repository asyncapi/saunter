# AsyncApi Generator.Build Nuget Package
A nuget package to generate AsyncAPI specification files at build time, based on code-first attributes. This nuget package requires .NET8.0 runtime in order to work. The consuming csproj doesn't need to target .NET8.0.

This nuget packages can help to better control API changes by commiting the AsyncAPI spec to source control. By always generating spec files at build, it will be clear when the api changes. 

# Customization Properties
The AsyncAPI spec generation can be configured through project properties in the csproj-file (or included via [.props files](https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build)):
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

- _AsyncAPIGenerateDocumentsOnBuild_: Whether to actually generate AsyncAPI spec files on build (true or false, default: true)
- _AsyncAPIDocumentFormats_: the output formats to generate, can be a combination of json, yml and/or yaml.
- _AsyncAPIDocumentOutputPath_: relative path where the AsyncAPI will be output (default is the csproj root path: ./)
- _AsyncAPIDocumentNames_: The AsyncAPI documents to generate. (default: generate all known documents)
- _AsyncAPIDocumentFilename_: the template for the outputted file names. Default: "{document}_asyncapi.{extension}" 
- _AsyncAPIDocumentEnvVars_: define environment variable(s) for the application. Formatted as a comma separated list of _key=value_ pairs, example: ```ASPNETCORE_ENVIRONMENT=AsyncAPI,CONNECT_TO_DATABASE=false```. 

None of these properties are mandatory. Only referencing the [AsyncAPI.Saunter.Generator.Build](https://www.nuget.org/packages/AsyncAPI.Saunter.Generator.Build) Nuget package will generate a json AsyncAPI spec file for all AsyncAPI documents.