# Build context: Repository-Root
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Projektdateien zuerst kopieren, damit der Restore-Layer gecacht werden kann.
COPY src/Calculator.Contracts/Calculator.Contracts.csproj src/Calculator.Contracts/
COPY src/Calculator.Server/Calculator.Server.csproj src/Calculator.Server/
RUN dotnet restore src/Calculator.Server/Calculator.Server.csproj

COPY src/Calculator.Contracts/ src/Calculator.Contracts/
COPY src/Calculator.Server/ src/Calculator.Server/
RUN dotnet publish src/Calculator.Server/Calculator.Server.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080 \
    Kestrel__EndpointDefaults__Protocols=Http1AndHttp2 \
    DOTNET_EnableDiagnostics=0
EXPOSE 8080
ENTRYPOINT ["dotnet", "Calculator.Server.dll"]
