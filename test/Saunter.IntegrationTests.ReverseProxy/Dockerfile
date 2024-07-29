FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["test/Saunter.IntegrationTests.ReverseProxy/Saunter.IntegrationTests.ReverseProxy.csproj", "test/Saunter.IntegrationTests.ReverseProxy/"]
COPY ["src/Saunter/Saunter.csproj", "src/Saunter/"]
RUN dotnet restore "./test/Saunter.IntegrationTests.ReverseProxy/Saunter.IntegrationTests.ReverseProxy.csproj"
COPY . .
WORKDIR "/src/test/Saunter.IntegrationTests.ReverseProxy"
RUN dotnet build "./Saunter.IntegrationTests.ReverseProxy.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Saunter.IntegrationTests.ReverseProxy.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Saunter.IntegrationTests.ReverseProxy.dll"]