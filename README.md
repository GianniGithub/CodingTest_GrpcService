# CodingTest Grpc Service

A compact calculator service as a coding test: The server provides arithmetic operations via **gRPC/Protocol Buffers**, while the standalone **Blazor WebAssembly client** offers a browser interface for it and uses gRPC-Web.

## Architecture Overview

```text
+---------------------------+       gRPC-Web        +----------------------------+
| Calculator.Client         |  ------------------->  | Calculator.Server          |
| Blazor WebAssembly        |                        | ASP.NET Core / Kestrel    |
| Browser                   |                        | CalculatorService         |
+-------------+-------------+                        +-------------+--------------+
              |                                                      |
              +------------------- Calculator.Contracts ------------+
                  calculator.proto + generated C# contracts
```

The client runs in the browser; the server performs the calculation. `Calculator.Contracts` is the shared contract layer generated from `calculator.proto`.

## Prerequisites

- **Local execution:** .NET 10 SDK
- **Docker execution:** Docker Desktop (Docker Compose is included); no local .NET or Node.js installations are required beyond that.

## Quick Start with Docker

From the repository root:

```bash
docker compose up --build
```

Afterwards, the Blazor application is available at <http://localhost:8081>. The server is additionally published for browser requests at <http://localhost:8080>.

By default, the Docker configuration uses `http://localhost:8080` as the server URL for the WASM client. This is intentionally an address reachable from the browser, and **not** `http://calculator-server:8080`: The client code runs in the browser, outside the Docker network. A different, browser-reachable URL can be set at build time:

```bash
docker compose build --build-arg GRPC_SERVER_URL=http://localhost:8080 calculator-client
docker compose up
```

## Local Development without Docker

Server and client are started in separate terminals from the repository root:

```bash
dotnet run --project ./src/Calculator.Server --launch-profile https

dotnet run --project ./src/Calculator.Client --launch-profile https
```

According to `launchSettings.json`, the HTTPS profiles use:

- Server: <https://localhost:7020> (HTTP alongside: <http://localhost:5109>)
- Client: <https://localhost:7244> (HTTP alongside: <http://localhost:5071>)

If the development certificate is not yet trusted:

```bash
dotnet dev-certs https --trust
```

The gRPC base address configured in the client must point to the server URL for the respective local execution. If the ports differ, the server's CORS rules are also required.

## Running Tests

```bash
cd ./Tests/Calculator.Server.Tests
dotnet test
```

The test project is an xUnit project and contains integration tests against the gRPC service (for example, via `WebApplicationFactory` or an in-memory test server).