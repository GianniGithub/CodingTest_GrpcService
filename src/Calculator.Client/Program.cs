using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Calculator.Client;
using Calculator.Contracts;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var grpcServerUrl = config["GrpcServerUrl"]
        ?? throw new InvalidOperationException("GrpcServerUrl ist nicht konfiguriert.");

    var handler = new GrpcWebHandler(
        GrpcWebMode.GrpcWeb,
        new HttpClientHandler());

    return GrpcChannel.ForAddress(grpcServerUrl, new GrpcChannelOptions
    {
        HttpHandler = handler
    });
});

builder.Services.AddScoped(sp =>
    new CalculatorService.CalculatorServiceClient(sp.GetRequiredService<GrpcChannel>()));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();