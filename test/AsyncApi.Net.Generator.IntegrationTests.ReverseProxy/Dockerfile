FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY bin/Release/net7.0/publish/ App/

WORKDIR /App

EXPOSE 5000

ENTRYPOINT ["dotnet", "AsyncApi.Net.Generator.IntegrationTests.ReverseProxy.dll"]
