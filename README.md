# Saunter

![CI](https://github.com/asyncapi/saunter/workflows/CI/badge.svg)
[![NuGet Badge](https://buildstats.info/nuget/saunter?includePreReleases=true)](https://www.nuget.org/packages/Saunter/)

Saunter is an [AsyncAPI](https://github.com/asyncapi/asyncapi) documentation generator for dotnet.


ℹ Note that pre version 1.0.0, the API is regarded as unstable and **breaking changes may be introduced**.


## Getting Started

See [examples/StreetlightsAPI](https://github.com/asyncapi/saunter/tree/main/examples/StreetlightsAPI).


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
        options.AssemblyMarkerTypes = new[] { typeof(StreetlightMessageBus) };
        
        // Build as much (or as little) of the AsyncApi document as you like.
        // Saunter will generate Channels, Operations, Messages, etc, but you
        // may want to specify Info here.
        options.Middleware.UiTitle = "Streetlights API";
        options.AsyncApi = new AsyncApiDocument
        {
            Info = new AsyncApiInfo()
            {
                Title = "Streetlights API",
                Version = "1.0.0",
                Description = "The Smartylighting Streetlights API allows you to remotely manage the city lights.",
                License = new AsyncApiLicense()
                {
                    Name = "Apache 2.0",
                    Url = new("https://www.apache.org/licenses/LICENSE-2.0"),
                }
            },
            Servers =
            {
                ["mosquitto"] = new AsyncApiServer(){ Url = "test.mosquitto.org",  Protocol = "mqtt"},
                ["webapi"] = new AsyncApiServer(){ Url = "localhost:5000",  Protocol = "http"},
            },
        };
    });
    ```

3. Add attributes to your classes which publish or subscribe to messages.

    ```csharp
    [AsyncApi] // Tells Saunter to scan this class.
    public class StreetlightMessageBus : IStreetlightMessageBus
    {
        private const string SubscribeLightMeasuredTopic = "subscribe/light/measured";
        
        [Channel(SubscribeLightMeasuredTopic, Servers = new[] { "mosquitto" })]
        [SubscribeOperation(typeof(LightMeasuredEvent), "Light", Summary = "Subscribe to environmental lighting conditions for a particular streetlight.")]
         public void PublishLightMeasuredEvent(Streetlight streetlight, int lumens) {}
    ```

4. Add saunter middleware to host the AsyncApi json document. In the `Configure` method of `Startup.cs`:

    ```csharp
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapAsyncApiDocuments();
        endpoints.MapAsyncApiUi();

        endpoints.MapControllers();
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

    ![AsyncAPI UI](https://raw.githubusercontent.com/asyncapi/saunter/main/assets/asyncapi-ui-screenshot.png)

## Configuration

See [the options source code](https://github.com/asyncapi/saunter/blob/main/src/Saunter/AsyncApiOptions.cs) for detailed info.

Common options are below:

```c#
services.AddAsyncApiSchemaGeneration(options =>
{
    options.AssemblyMarkerTypes = new[] { typeof(Startup) };   // Tell Saunter where to scan for your classes.
    
    options.AddChannelFilter<MyAsyncApiChannelFilter>();       // Dynamically update ChanelItems
    options.AddOperationFilter<MyOperationFilter>();           // Dynamically update Operations
    
    options.Middleware.Route = "/asyncapi/asyncapi.json";      // AsyncAPI JSON document URL
    options.Middleware.UiBaseRoute = "/asyncapi/ui/";          // AsyncAPI UI URL
    options.Middleware.UiTitle = "My AsyncAPI Documentation";  // AsyncAPI UI page title
}
```

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
* [AMQP](https://github.com/asyncapi/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Amqp)
* [HTTP](https://github.com/asyncapi/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Http)
* [Kafka](https://github.com/asyncapi/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Kafka)
* [MQTT](https://github.com/asyncapi/saunter/tree/main/src/Saunter/AsyncApiSchema/v2/Bindings/Mqtt)

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

```
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

See our [contributing guide](https://github.com/asyncapi/saunter/blob/main/CONTRIBUTING.md/CONTRIBUTING.md).

Feel free to get involved in the project by opening issues, or submitting pull requests.

You can also find me on the [AsyncAPI community slack](https://asyncapi.com/slack-invite).

## Thanks

* This project is heavily inspired by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).
* We use [LEGO AsyncAPI.NET](https://github.com/LEGO/AsyncAPI.NET) schema and serializing.
