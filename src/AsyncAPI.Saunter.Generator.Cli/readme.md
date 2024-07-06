# AsyncApi Generator.Cli Tool
A dotnet tool to generate AsyncAPI specification files based of dotnet DLL (The application itself).

## Tool usage
```
dotnet asyncapi.net tofile --output [output-path] --format [json,yml,yaml] --doc [asyncapi-document-name] [startup-assembly]
```
startup-assembly: the file path to the entrypoint dotnet DLL that hosts AsyncAPI document(s).

## Tool options
--doc: The name of the AsyncAPI document as defined in the startup class by the ```.ConfigureNamedAsyncApi()```-method. If not specified, all documents will be exported.
--output: relative path where the AsyncAPI will be output [defaults to stdout]
--filename: the template for the outputted file names. Default: "{document}_asyncapi.{extension}"
--format: the output formats to generate, can be a combination of json, yml and/or yaml. File extension is appended to the output path.
--env: define environment variable(s) for the application
