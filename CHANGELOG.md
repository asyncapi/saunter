# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)

<!-- Please update the links section at the bottom when adding a new version. -->

## [Unreleased]


## [v0.3]
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
[Unreleased]: https://github.com/tehmantra/saunter/compare/v0.3.0...master
[v0.3.0]: https://github.com/tehmantra/saunter/compare/v0.2.0...v0.3.0
[v0.2.0]: https://github.com/tehmantra/saunter/compare/v0.1.0...v0.2.0
[v0.1.0]: https://github.com/tehmantra/saunter/compare/97abfdb20e11dccfe4c6b9317e6a7e1fa419fd5c...v0.1.0