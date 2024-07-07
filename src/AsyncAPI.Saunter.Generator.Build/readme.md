# AsyncApi Generator.Build Nuget Package
A nuget package to generate AsyncAPI specification files at build time, based on code-first attributes.

This nuget packages can help to better control API changes by commiting the AsyncAPI spec to source control. By always generating spec files at build, it will be clear when the api changes. 
Example to include the Generator.Build nuget package only in (local) debug builds:
```
  <ItemGroup Condition=" '$(Configuration)' == 'Debug' ">
    <PackageReference Include="AsyncAPI.Saunter.Generator.Build" Version="1.*">
      <PrivateAssets>all</PrivateAssets>
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
    </PackageReference>
  </ItemGroup>
```

# Customization Properties
The AsyncAPI spec generation can be configured through project properties in the csproj-file (or included via .props files):
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
- _AsyncAPIDocumentFormats_: Format of the expected AsyncAPI spec files (json, yml or yaml, default: json)
- _AsyncAPIDocumentOutputPath_: Output path for the AsyncAPI spec files, relative to the csproj location. (default is the csproj root path: ./)
- _AsyncAPIDocumentNames_: The AsyncAPI documents to generate. (default: generate all known documents)
- _AsyncAPIDocumentFilename_: Template of the AsyncAPI spec files (default: "{document}_asyncapi.{extension}")
- _AsyncAPIDocumentEnvVars_: Environment variable(s) to set during generation of the AsyncAPI spec files (default: none, Example: "ASPNETCORE_ENVIRONMENT=Development")  

None of these properties are mandatory. Only referencing the [AsyncAPI.Saunter.Generator.Build](https://www.nuget.org/packages/AsyncAPI.Saunter.Generator.Build) Nuget package will generate a json AsyncAPI spec file for all AsyncAPI documents.