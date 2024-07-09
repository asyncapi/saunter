FROM mcr.microsoft.com/dotnet/aspnet:6.0
# Run "dotnet publish -c Release" before building this image
COPY bin/Release/net6.0/publish/ App/

WORKDIR /App
EXPOSE 5000
ENTRYPOINT ["dotnet", "Saunter.IntegrationTests.ReverseProxy.dll"]
