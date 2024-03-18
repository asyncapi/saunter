# Saunter.Cli

A simple utility for generating async api specifications from a set of dlls.

## Idea

For aot and trim assemblies, it is inject at the post-build stage, сreates a json document that is included in the build and used by the running application.

## Usage

So far, just an example:

```bash
saunter-cli specification generate -p saunter/examples/StreetlightsAPI/bin/Release/net6.0/ --prototype '{ "id": "tester" }'
```
