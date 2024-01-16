# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

<!-- Please update the links section at the bottom when adding a new version. -->


## [v0.13.0] - 2024-01-16
### Changed
- [Updated NJsonSchema to v11.0.0](https://github.com/m-wild/saunter/issues/179)
- [Document names are correctly copied to clones when using named docs](https://github.com/m-wild/saunter/issues/172)
- Updated @asyncapi/react-component to v1.2.11
- Use npm-ci instead of npm-install in ci scripts

## [v0.12.0] - 2023-06-15
### Added
- [Add support for message headers](https://github.com/tehmantra/saunter/issues/150)
### Changed
- Updated @asyncapi/react-component to v1.0.0-next.48
- Updated NJsonSchema dependencies ([fixes compatibility with NSwag](https://github.com/tehmantra/saunter/issues/156))
### Fixed
- [Duplicated operation when types of the same assembly are used in AssemblyMarkerTypes](https://github.com/tehmantra/saunter/issues/163)
- [Tags do not work without description and external docs](https://github.com/tehmantra/saunter/issues/149)
  - Updates Tag Description and ExternalDocs to ignore nulls to eliminate async api parser errors.
  - Adds support for Tags from a class that was missing.

## [v0.11.0] - 2022-10-03
### Added
- Message and Operation attributes now allow setting tags

## [v0.10.0] - 2022-08-22
### Changed
- AsyncAPI spec version bumped to 2.4.0
- Added messageId to Message and MessageAttribute 
- Updated @asyncapi/react-component to v1.0.0-next.40
- Target net6.0 only - multitargeting was painful to test & maintain
- Updated NJsonSchema to v10.7.2
- [Add README to NuGet package #110](https://github.com/tehmantra/saunter/issues/110)

### Fixed
- [Saunter breaks when you try register two same channels #133](https://github.com/tehmantra/saunter/issues/133)
- [NJsonSchema uses unsupported Json Schema. #138](https://github.com/tehmantra/saunter/issues/138)
- [Use NJsonSchema for the JSON Schema implementation #60](https://github.com/tehmantra/saunter/issues/60)
- [Error: Maximum call stack size exceeded #123](https://github.com/tehmantra/saunter/issues/123)

## [v0.9.1] - 2021-11-08
### Fixed
- Hosting behind a reverse proxy now works correctly. See tests/Saunter.IntegrationTests.ReverseProxy/README.md for an example.

## [v0.9.0] - 2021-10-17
### Changed
- AsyncAPI spec version bumped to 2.2.0
  - New optional property `string[] Servers` available on `ChannelAttribute`
  - New optional property `List<string> Servers` available on `ChannelItem`
- Bump UI library to v1.0.0-next.21

## [v0.8.0] - 2021-09-11
### Changed
- Filters are now registered as types (e.g. `options.AddOperationFilter<T>`) and resolved from an `IServiceProvider`

## [v0.7.1] - 2021-08-04
### Fixed
- Include XML documentation file in build (HOW HAD I NOT NOTICED THIS BEFORE?!)

## [v0.7.0] - 2021-08-04
### Changed
- AsyncAPI spec version bumped to 2.1.0
- `Message.Examples` type changed from `IList<IDictionary<string, object>>` to `IList<MessageExample>` to allow specifying the new `name` and `summary` fields.
- New security schemes added.

## [v0.6.0] - 2021-08-03
### Added
- Added ability to generate multiple AsyncAPI documents using the `ConfigureNamedAsyncApi` extension


## [v0.5.0] - 2021-07-28
### Changed
- Added ability to use bindings by specifying a BindingsRef in the attribute e.g. `[Channel("light.measured", BindingsRef = "my-amqp-binding")]`
- Added MQTT bindings
- Swapped custom JSON schema generation for NJsonSchema
- Removed direct dependencies on System.Text.Json



## [v0.4.0] - 2021-07-27
### Changed
- UI no longer proxies to playground.asyncapi.io, but instead uses the asyncapi standalone react component as an embedded resource.
- Removed settings related to the proxied UI
    - AsyncApi.Middleware.UiRoute
    - AsyncApi.Middleware.PlaygroundBaseAddress
- Bumped project dependencies 
```
System.Text.Json 5.0.0 -> 5.0.2
Namotion.Reflection 1.0.14 -> 1.0.23
Microsoft.NET.Test.Sdk 16.8.3 -> 16.10.0
```


## [v0.3.1] - 2021-07-19
### Fixed
- DateTimeOffset causes recursive schema - now treated same as DateTime
- DictionaryKeyToStringConverter to support lists #94

## [v0.3.0] - 2021-07-19
### Changed
- Replaced dependency on Newtonsoft.Json with System.Text.Json
    - Previously we were inspecting types for the attributes provided by Newtonsoft.Json such as `[JsonProperty]`. If you were relying on these attributes, you will now need to set `options.SchemaIdSelector` and/or `options.PropertyNameSelector` to a function which inspects those attributes.
    e.g.
    ```csharp
    services.AddAsyncApiSchemaGeneration(options =>
    {
        options.PropertyNameSelector = prop => 
            prop.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? prop.Name;
    });
    ```

### Added
- Multi-targeting `netcoreapp3.1`, `net5.0`  and `netstandard2.0`
- Async API UI
- Endpoint-aware middleware on `netcoreapp3.1`+
    ```csharp
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapAsyncApiDocuments();
        endpoints.MapAsyncApiUi();
    })
    ```
- Bindings for HTTP, Kafka & RabbitMQ
- Support for Subscribe and Publish operations of the same Channel in different classes
- Code-first discriminator support with `[Discriminator]` and `[DiscriminatorSubType]` attributes
- Support for `[Required]` attribute
- Support for Channel Parameters
- Support for `oneOf` message types (use multiple `[Message(Type)]` attributes)

### Fixed
- `AsyncApiOptions.PropertyNameSelector` was not being used


## [v0.2.0] - 2020-08-04
### Added
- Support System.Guid as a JSON Schema string with "uuid" format.
- Default Schema ID factory handles generics in a human-friendly format 
    - `List<Foo>` becomes `"listOfFoo"`
    - `Dictionary<string, Foo>` becomes `"dictionaryOfStringAndFoo"`
    - etc

## [v0.1.0] - 2020-07-02
### Changed
- First stable release!


<!--
When updating here set baseVersion to the previous tag and targetVersion to your new tag
This link will be dead until after you have completed the pull request and tagged the new version in master
-->

[v0.11.1]: https://github.com/tehmantra/saunter/compare/v0.11.0...v0.11.1
[v0.11.0]: https://github.com/tehmantra/saunter/compare/v0.10.0...v0.11.0
[v0.10.0]: https://github.com/tehmantra/saunter/compare/v0.9.1...v0.10.0
[v0.9.1]: https://github.com/tehmantra/saunter/compare/v0.9.0...v0.9.1
[v0.9.0]: https://github.com/tehmantra/saunter/compare/v0.8.0...v0.9.0
[v0.8.0]: https://github.com/tehmantra/saunter/compare/v0.7.1...v0.8.0
[v0.7.1]: https://github.com/tehmantra/saunter/compare/v0.7.0...v0.7.1
[v0.7.0]: https://github.com/tehmantra/saunter/compare/v0.6.0...v0.7.0
[v0.6.0]: https://github.com/tehmantra/saunter/compare/v0.5.0...v0.6.0
[v0.5.0]: https://github.com/tehmantra/saunter/compare/v0.4.0...v0.5.0
[v0.4.0]: https://github.com/tehmantra/saunter/compare/v0.3.1...v0.4.0
[v0.3.1]: https://github.com/tehmantra/saunter/compare/v0.3.0...v0.3.1
[v0.3.0]: https://github.com/tehmantra/saunter/compare/v0.2.0...v0.3.0
[v0.2.0]: https://github.com/tehmantra/saunter/compare/v0.1.0...v0.2.0
[v0.1.0]: https://github.com/tehmantra/saunter/compare/97abfdb20e11dccfe4c6b9317e6a7e1fa419fd5c...v0.1.0
