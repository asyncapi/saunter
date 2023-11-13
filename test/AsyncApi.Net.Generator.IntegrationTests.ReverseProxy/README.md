Integration test for ensuring AsyncApi.Net.Generator works correctly hosted behind a reverse proxy.

The [docker-compose.yml](./docker-compose.yml) file sets up 3 containers
1. service-a
2. service-b
3. nginx reverse proxy


Running the test:
```
$ dotnet publish -c Release
$ docker-compose up
```


You should be able to access both services UI
* http://localhost:5000/service-a/asyncapi/ui
* http://localhost:5000/service-b/asyncapi/ui
