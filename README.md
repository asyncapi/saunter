# Saunter

![CI](https://github.com/tehmantra/saunter/workflows/CI/badge.svg)
[![NuGet Badge](https://buildstats.info/nuget/saunter?includePreReleases=true)](https://www.nuget.org/packages/Saunter/)

Saunter is an [AsyncAPI](https://github.com/asyncapi/asyncapi) documentation generator for dotnet.


ℹ Note that pre version 1.0.0, the API is regarded as unstable and **breaking changes may be introduced**.


## Getting Started

See [examples/StreetlightsAPI](https://github.com/tehmantra/saunter/blob/main/examples/StreetlightsAPI).


1. Install the Saunter package

    ```
    dotnet add package Saunter
    ```

2. In the `ConfigureServices` method of `Startup.cs`, configure Saunter.

    ```csharp
    // Add Saunter to the application services. 
    services.AddAsyncApiSchemaGeneration(options =>
    {
        // Specify example type(s) from assemblies to scan.
        options.AssemblyMarkerTypes = new[] {typeof(StreetlightMessageBus)};

        // Build as much (or as little) of the AsyncApi document as you like.
        // Saunter will generate Channels, Operations, Messages, etc, but you
        // may want to specify Info here.
        options.AsyncApi = new AsyncApiDocument
        {
            Info = new Info("Streetlights API", "1.0.0")
            {
                Description = "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
                License = new License("Apache 2.0")
                {
                    Url = "https://www.apache.org/licenses/LICENSE-2.0"
                }
            },
            Servers =
            {
                { "mosquitto", new Server("test.mosquitto.org", "mqtt") }
            }
        };
    });
    ```

3. Add attributes to your classes which publish or subscribe to messages.

    ```csharp
    [AsyncApi] // Tells Saunter to scan this class.
    public class StreetlightMessageBus : IStreetlightMessageBus
    {
        [Channel("publish/light/measured")] // Creates a Channel
        [PublishOperation(typeof(LightMeasuredEvent), Summary = "Inform about environmental lighting conditions for a particular streetlight.")] // A simple Publish operation.
        public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens) {}
    ```

4. Add saunter middleware to host the AsyncApi json document. In the `Configure` method of `Startup.cs`:

    ```csharp
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapAsyncApiDocuments();
        endpoints.MapAsyncApiUi();
    });
    ```

5. Use the published AsyncApi document:

    ```jsonc
    // HTTP GET /asyncapi/asyncapi.json
    {
        // Properties from Startup.cs
        "asyncapi": "2.1.0",
        "info": {
            "title": "Streetlights API",
            "version": "1.0.0",
            "description": "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
           // ...
        },
        // Properties generated from Attributes
        "channels": {
            "light/measured": {
            "publish": {
                "operationId": "PublishLightMeasuredEvent",
                "summary": "Inform about environmental lighting conditions for a particular streetlight.",
            //...
    }
    ```
   
6. Use the published AsyncAPI UI:

    ![AsyncAPI UI](https://raw.githubusercontent.com/tehmantra/saunter/main/assets/asyncapi-ui-screenshot.png)

## Configuration

See [the options source code](https://github.com/tehmantra/saunter/blob/main/src/Saunter/AsyncApiOptions.cs) for detailed info.

Common options are below:

```c#
services.AddAsyncApiSchemaGeneration(options =>
{
    options.AssemblyMarkerTypes = new[] { typeof(Startup) };   // Tell Saunter where to scan for your classes.
    
    options.AddChannelItemFilter<MyChannelItemFilter>();       // Dynamically update ChanelItems
    options.AddOperationFilter<MyOperationFilter>();           // Dynamically update Operations
    
    options.Middleware.ReverseProxyBasePath = "/";             // BasePath of the application in a proxy-reverse environment.
    options.Middleware.Route = "/asyncapi/asyncapi.json";      // AsyncAPI JSON document URL.
    options.Middleware.UiBaseRoute = "/asyncapi/ui/";          // AsyncAPI UI URL
    options.Middleware.UiTitle = "My AsyncAPI Documentation";  // AsyncAPI UI page title
}
```


## JSON Schema Settings

The JSON schema generation can be customized using the `options.JsonSchemaGeneratorSettings`. Saunter defaults to the popular `camelCase` naming strategy for both properties and types.

For example, setting to use PascalCase:

```c#
services.AddAsyncApiSchemaGeneration(options =>
{
    options.JsonSchemaGeneratorSettings.TypeNameGenerator = new DefaultTypeNameGenerator();

    // Note: need to assign a new JsonSerializerSettings, not just set the properties within it.
    options.JsonSchemaGeneratorSettings.SerializerSettings = new JsonSerializerSettings 
    {
        ContractResolver = new DefaultContractResolver(),
        Formatting = Formatting.Indented;
    };
}
```

You have access to the full range of both [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) and [JSON.NET](https://github.com/JamesNK/Newtonsoft.Json) settings to configure the JSON schema generation, including custom ContractResolvers.


## Bindings

Bindings are used to describe protocol specific information. These can be added to the AsyncAPI document and then applied to different components by setting the `BindingsRef` property in the relevant attributes `[OperationAttribute]`, `[MessageAttribute]`, `[ChannelAttribute]`


```csharp
// Startup.cs
services.AddAsyncApiSchemaGeneration(options =>
{
    options.AsyncApi = new AsyncApiDocument
    {
        Components = 
        {
            ChannelBindings = 
            {
                ["my-amqp-binding"] = new ChannelBindings
                {
                    Amqp = new AmqpChannelBinding
                    {
                        Is = AmqpChannelBindingIs.RoutingKey,
                        Exchange = new AmqpChannelBindingExchange
                        {
                            Name = "example-exchange",
                            VirtualHost = "/development"
                        }
                    }
                }
            }
        }
    }
});
```

```csharp
[Channel("light.measured", BindingsRef = "my-amqp-binding")] // Set the BindingsRef property
public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens) {}
```

Available bindings:
* [AMQP](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Amqp)
* [HTTP](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Http)
* [Kafka](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Kafka)
* [MQTT](https://github.com/tehmantra/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Mqtt)


## Multiple AsyncAPI documents

You can generate multiple AsyncAPI documents by using the `ConfigureNamedAsyncApi` extension method.

```cs
// Startup.cs

// Add Saunter to the application services. 
services.AddAsyncApiSchemaGeneration(options =>
{
    // Specify example type(s) from assemblies to scan.
    options.AssemblyMarkerTypes = new[] {typeof(FooMessageBus)};
}

// Configure one or more named AsyncAPI documents
services.ConfigureNamedAsyncApi("Foo", asyncApi => 
{
    asyncApi.Info = new Info("Foo API", "1.0.0");
    // ...
});

services.ConfigureNamedAsyncApi("Bar", asyncApi => 
{
    asyncApi.Info = new Info("Bar API", "1.0.0");
    // ...
});
```

Classes need to be decorated with the `AsyncApiAttribute` specifying the name of the AsyncAPI document.

```cs
[AsyncApi("Foo")]
public class FooMessageBus 
{
    // Any channels defined in this class will be added to the "Foo" document
}


[AsyncApi("Bar")]
public class BarMessageBus 
{
    // Any channels defined in this class will be added to the "Bar" document
}
```

Each document can be accessed by specifying the name in the URL

```json
// GET /asyncapi/foo/asyncapi.json
{
    "info": {
        "title": "Foo API"
    }
}

// GET /asyncapi/bar/asyncapi.json
{
    "info": {
        "title": "Bar API"
    }
}
```


## Contributing

See our [contributing guide](https://github.com/tehmantra/saunter/blob/main/CONTRIBUTING.md/CONTRIBUTING.md).

Feel free to get involved in the project by opening issues, or submitting pull requests.

You can also find me on the [AsyncAPI community slack](https://asyncapi.com/slack-invite).

## Thanks

* This project is heavily inspired by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).
* We use [NJsonSchema](https://github.com/RicoSuter/NJsonSchema) for the JSON schema heavy lifting, 
* and [Namotion.Reflection](https://github.com/RicoSuter/Namotion.Reflection) for pulling XML documentation.

