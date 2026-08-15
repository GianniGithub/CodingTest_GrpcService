using Calculator.Contracts;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
namespace Calculator.Server.Tests.Integration;

[Trait("Category", "Integration")]
public class CalculatorGrpcIntegrationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{

    [Fact]
    public async Task Add_ReturnsExpectedResult_ThroughGrpc()
    {
        var client = factory.CreateDefaultClient();

        var channel = GrpcChannel.ForAddress(
            client.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = client
            });

        var grpcClient = new CalculatorService.CalculatorServiceClient(channel);

        var request = new BinaryOperationRequest
        {
            Left = 5,
            Right = 7
        };

        // Act
        var response = await grpcClient.AddAsync(request);
        
        Assert.Equal(12, response.Value);
    }
}