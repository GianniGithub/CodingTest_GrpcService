# Build context: Repository-Root
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Der Browser erreicht den Server über diese URL (nicht über den Compose-Service-Namen).
ARG GRPC_SERVER_URL=http://localhost:8080

COPY src/Calculator.Contracts/Calculator.Contracts.csproj src/Calculator.Contracts/
COPY src/Calculator.Client/Calculator.Client.csproj src/Calculator.Client/
RUN dotnet restore src/Calculator.Client/Calculator.Client.csproj

COPY src/Calculator.Contracts/ src/Calculator.Contracts/
COPY src/Calculator.Client/ src/Calculator.Client/

# Blazor WASM lädt appsettings.json aus wwwroot über IConfiguration.
# Die Docker-Base-Adresse wird daher beim Image-Build in die veröffentlichte App gelegt.
RUN printf '{"GrpcServerUrl":"%s"}\n' "$GRPC_SERVER_URL" > src/Calculator.Client/wwwroot/appsettings.json
RUN dotnet publish src/Calculator.Client/Calculator.Client.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore

FROM nginx:alpine AS runtime
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
