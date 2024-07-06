# AsyncApi Generator.Cli Tool

## Tool usage
```
dotnet asyncapi tofile --output [output-path] --format [json,yml,yaml] [startup-assembly] [asyncapi-document-name]
```

## Tool options
startup-assembly: the file path to the entrypoint dotnet DLL that hosts AsyncAPI document(s).
asyncapi-document-name: (optional) The name of the AsyncAPI document as defined in the startup class by the ```.ConfigureNamedAsyncApi()```-method. If not specified, all documents will be exported.

--output: the output path or the file name. File extension can be omitted, as the --format file determine the file extension.
--format: the output formats to generate, can be a combination of json, yml and/or yaml. File extension is appended to the output path.
