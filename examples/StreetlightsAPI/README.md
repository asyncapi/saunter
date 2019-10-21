# Streetlights API Example

This is an example implementation of the [Streetlights API from the asyncapi tutorial](https://www.asyncapi.com/docs/tutorials/streetlights/).

The generated AsyncAPI documentation should look like this:

```yml
asyncapi: '2.0.0'
info:
  title: Streetlights API
  version: '1.0.0'
  description: |
    The Smartylighting Streetlights API allows you
    to remotely manage the city lights.
  license:
    name: Apache 2.0
    url: 'https://www.apache.org/licenses/LICENSE-2.0'

servers:
  mosquitto:
    url: test.mosquitto.org
    protocol: mqtt

channels:
  light/measured:
    publish:
      summary: Inform about environmental lighting conditions for a particular streetlight.
      message:
        payload:
          type: object
          properties:
            id:
              type: integer
              minimum: 0
              description: Id of the streetlight.
            lumens:
              type: integer
              minimum: 0
              description: Light intensity measured in lumens.
            sentAt:
              type: string
              format: date-time
              description: Date and time when the message was sent.
```

## Running

Run the sample with `dotnet run` and then use curl (or similar) to send test requests to the API.

```PowerShell
# Run the API
$ dotnet run

info: Microsoft.Hosting.Lifetime[0]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Production
info: Microsoft.Hosting.Lifetime[0]
      Content root path: ~/saunter/examples/StreetlightsAPI


# Get streetlights
Invoke-WebRequest "http://localhost:5000/api/streetlights" | ConvertFrom-Json

id position
-- --------
 1 {-36.32032, 175.485986}

# Request light measurement
$ Invoke-WebRequest -Method POST -Uri "http://localhost:5000/api/streetlights/1/measure-light" | Select StatusCode
StatusCode
----------
       200

# In the logs for the API we should see our message being published

info: Streetlight[0]
      Publishing message {"Id":1,"Lumens":425,"SentAt":"2019-10-04T22:22:08.884346+13:00"} to light/measured
      
# Get the AsyncAPI Schema document
$ Invoke-WebRequest -Method GET -Uri "http://localhost:5000/asyncapi/asyncapi.json"
```

The generated asyncapi document, not identical to the one above, as schemas are moved to the `components` section of the document.

```json
{
  "asyncapi": "2.0.0",
  "info": {
    "title": "Streetlights API",
    "version": "1.0.0",
    "description": "The Smartylighting Streetlights API allows you\nto remotely manage the city lights.",
    "license": {
      "name": "Apache 2.0",
      "url": "https://www.apache.org/licenses/LICENSE-2.0"
    }
  },
  "servers": {
    "mosquitto": {
      "url": "test.mosquitto.org",
      "protocol": "mqtt"
    }
  },
  "defaultContentType": "application/json",
  "channels": {
    "light/measured": {
      "publish": {
        "operationId": "PublishLightMeasuredEvent",
        "summary": "Inform about environmental lighting conditions for a particular streetlight.",
        "message": {
          "payload": {
            "$ref": "#/components/schemas/LightMeasuredEvent"
          }
        }
      }
    }
  },
  "components": {
    "schemas": {
      "LightMeasuredEvent": {
        "title": "LightMeasuredEvent",
        "type": "object",
        "properties": {
          "id": {
            "type": "integer",
            "description": "Id of the streetlight.",
            "format": "int32"
          },
          "lumens": {
            "type": "integer",
            "description": "Light intensity measured in lumens.",
            "format": "int32"
          },
          "sentAt": {
            "type": "string",
            "description": "Light intensity measured in lumens.",
            "format": "date-time"
          }
        }
      }
    }
  },
  "tags": [
    
  ]
}
```
