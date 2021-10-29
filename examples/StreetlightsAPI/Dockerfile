FROM mcr.microsoft.com/dotnet/aspnet:5.0

# Run "dotnet publish -c Release" before build this image
COPY bin/Release/net5.0/publish/ App/

WORKDIR /App

EXPOSE 5000

ENTRYPOINT ["dotnet", "StreetlightsAPI.dll"]
