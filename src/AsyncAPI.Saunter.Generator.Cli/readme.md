# AsyncApi Generator.Cli Tool
A dotnet tool to generate AsyncAPI specification files based of a dotnet assembly (The application itself).

This tool can used to manually generate spec files from a startup assembly or to setup a custom build pipeline.
If you are intressed in always generating spec files at build time, see [AsyncAPI.Saunter.Generator.Build](https://www.nuget.org/packages/AsyncAPI.Saunter.Generator.Build). 

## Tool usage
```
dotnet asyncapi tofile [startup-assembly] --output [output-path] --format [json,yml,yaml] --doc [asyncapi-document-name]
```
- _startup-assembly_: the file path to the dotnet startup assembly (DLL) that hosts AsyncAPI document(s).

## Tool options
- _--doc_: The name of the AsyncAPI document as defined in the startup class by the ```.ConfigureNamedAsyncApi()```-method. If only ```.AddAsyncApiSchemaGeneration()``` is used, the document is unnamed and will always be exported. If not specified, all documents will be exported.  
- _--output_: relative path where the AsyncAPI documents will be exported to (Default: the csproj root "./"). 
- _--filename_: the template for the outputted file names (Default: "{document}_asyncapi.{extension}").  
- _--format_: the output formats to generate, can be a combination of json, yml and/or yaml (Default: "json").
- _--env_: define environment variable(s) for the application. Formatted as a comma separated list of _key=value_ pairs, example: ```ASPNETCORE_ENVIRONMENT=AsyncAPI,CONNECT_TO_DATABASE=false```.  

## Install the Generator.Cli dotnet Tool
```
dotnet tool install --global AsyncAPI.Saunter.Generator.Cli
```
After installing the tool globally, it is available using commands: ```dotnet asyncapi``` or ```dotnet-asyncapi```

Want to learn more about .NET tools? Or want to install it local using a manifest?
[Check out this Microsoft page on how to manage .NET tools](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools)

## Internals
How does the tool work internally? It tries to exact an ```IServiceProvider``` from the provided _startup-assembly_ and exports AsyncApiDocument(s) as registered with the services provider.