Integration test for ensuring Saunter works correctly hosted behind a reverse proxy.

The [docker-compose.yml](./docker-compose.yml) file sets up 3 containers
1. service-a
2. service-b
3. nginx reverse proxy

Running the test (from root project location):

```bash
docker-compose --file ./test/Saunter.IntegrationTests.ReverseProxy/docker-compose.yml up --build
```

You should be able to access both services UI
* http://localhost:5000/service-a/asyncapi/ui
* http://localhost:5000/service-b/asyncapi/ui
