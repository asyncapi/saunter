# Streetlights API Example

This is an example implementation (with minor additions) of the [Streetlights API from the asyncapi tutorial](https://www.asyncapi.com/docs/tutorials/streetlights/).

## Running

The example project references the Saunter project directly (for easy debugging), so we need to install the UI assets manually.
This requires nodejs/npm, but neither are required when using the nuget package in your project.

Run the sample with `dotnet run` and then use curl (or similar) to send test requests to the API.

```sh
#### Install UI assets

$ cd ~/saunter/src/Saunter.UI
$ npm install

added 106 packages, and audited 107 packages in 2s

1 package is looking for funding
  run `npm fund` for details

found 0 vulnerabilities

#### Run the example

$ cd ~/saunter/examples/StreetlightsAPI
$ dotnet run

info: StreetlightsAPI.Program[0] AsyncAPI doc available at: http://localhost:5000/asyncapi/asyncapi.json
info: StreetlightsAPI.Program[0] AsyncAPI UI available at: http://localhost:5000/asyncapi/ui/
info: Microsoft.Hosting.Lifetime[0] Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0] Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0] Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0] Content root path: saunter\examples\StreetlightsAPI

#### View the UI in your browser at http://localhost:5000/asyncapi/ui/

#### Get streetlights

$ Invoke-WebRequest 'http://localhost:5000/api/streetlights' | ConvertFrom-Json

id position                lightIntensity
-- --------                --------------
 1 {-36.32032, 175.485986} {}

#### Publish an example light measurement

$ Invoke-WebRequest -Method POST -Uri 'http://localhost:5000/publish/light/measured' -Body '{"id":1, "lumens":400}' -ContentType 'application/json' | Select StatusCode

StatusCode
----------
       200

#### In the logs for the API we should see our message being subscribed & published

StreetlightsAPI.StreetlightsController[0] Received message on publish/light/measured with payload {"Id":1,"Lumens":400,"SentAt":"2021-09-11T00:52:55.4171809+12:00"}
info: Streetlight[0] Publishing message {"Id":1,"Lumens":400,"SentAt":"2021-09-11T00:52:55.4171809+12:00"} to test.mosquitto.org/subscribe/light/measured
      
#### Get the AsyncAPI Schema document

$ Invoke-WebRequest -Method GET -Uri 'http://localhost:5000/asyncapi/asyncapi.json'
```

The generated asyncapi document is not identical to the AsyncAPI example above as schemas are automatically moved to the `components` section of the document.

```json
{
  "asyncapi": "2.6.0",
  "info": {
    "title": "Streetlights API",
    "version": "1.0.0",
    "description": "The Smartylighting Streetlights API allows you to remotely manage the city lights.",
    "license": {
      "name": "Apache 2.0",
      "url": "https://www.apache.org/licenses/LICENSE-2.0"
    }
  },
  "servers": {
    "mosquitto": {
      "url": "test.mosquitto.org",
      "protocol": "mqtt"
    },
    "webapi": {
      "url": "localhost:5000",
      "protocol": "http"
    }
  },
  "defaultContentType": "application/json",
  "channels": {
    "publish/light/measured": {
      "servers": [
        "webapi"
      ],
      "publish": {
        "operationId": "MeasureLight",
        "summary": "Inform about environmental lighting conditions for a particular streetlight.",
        "tags": [
          {
            "name": "Light"
          }
        ],
        "bindings": {
          "$ref": "#/components/operationBindings/postBind"
        },
        "message": {
          "$ref": "#/components/messages/lightMeasuredEvent"
        }
      }
    },
    "subscribe/light/measured": {
      "servers": [
        "mosquitto"
      ],
      "subscribe": {
        "operationId": "PublishLightMeasurement",
        "summary": "Subscribe to environmental lighting conditions for a particular streetlight.",
        "tags": [
          {
            "name": "Light"
          }
        ],
        "message": {
          "$ref": "#/components/messages/lightMeasuredEvent"
        }
      },
      "bindings": {
        "$ref": "#/components/channelBindings/amqpDev"
      }
    }
  },
  "components": {
    "schemas": {
      "lightMeasuredEvent": {
        "title": "lightMeasuredEvent",
        "type": "object",
        "properties": {
          "id": {
            "title": "int32",
            "type": "integer",
            "format": "int32"
          },
          "lumens": {
            "title": "int32",
            "type": "integer",
            "format": "int32"
          },
          "sentAt": {
            "title": "dateTime",
            "type": "string",
            "format": "dateTime"
          }
        },
        "nullable": true
      },
      "int32": {
        "title": "int32",
        "type": "integer",
        "format": "int32"
      },
      "dateTime": {
        "title": "dateTime",
        "type": "string",
        "format": "dateTime"
      }
    },
    "messages": {
      "lightMeasuredEvent": {
        "payload": {
          "$ref": "#/components/schemas/lightMeasuredEvent"
        },
        "name": "lightMeasuredEvent",
        "title": "lightMeasuredEvent"
      }
    },
    "channelBindings": {
      "amqpDev": {
        "amqp": {
          "is": "queue"
        }
      }
    },
    "operationBindings": {
      "postBind": {
        "http": {
          "type": "response",
          "method": "POST"
        }
      }
    }
  }
}
```