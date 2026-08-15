using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Calculator.Client;
using Calculator.Contracts;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var grpcServerUrl = config["GrpcServerUrl"]
        ?? throw new InvalidOperationException("GrpcServerUrl is not set.");

    var handler = new GrpcWebHandler(
        GrpcWebMode.GrpcWeb,
        new HttpClientHandler());

    return GrpcChannel.ForAddress(grpcServerUrl, new GrpcChannelOptions
    {
        HttpHandler = handler
    });
});

// Typisierter gRPC-Client, der den Channel oben injiziert bekommt
builder.Services.AddScoped(sp =>
    new CalculatorService.CalculatorServiceClient(
        sp.GetRequiredService<GrpcChannel>()));

await builder.Build().RunAsync();