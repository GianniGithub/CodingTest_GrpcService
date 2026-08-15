using Calculator.Contracts;
using Grpc.Core;
// = dein option csharp_namespace

namespace Calculator.Server.Services;

public class CalculatorGrpcService : CalculatorService.CalculatorServiceBase
{
    private readonly ILogger<CalculatorGrpcService> _logger;

    public CalculatorGrpcService(ILogger<CalculatorGrpcService> logger)
        => _logger = logger;

    public override Task<CalculationResponse> Add(
        BinaryOperationRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Add {Left} + {Right}", request.Left, request.Right);
        return Task.FromResult(new CalculationResponse
        {
            Value = request.Left + request.Right
        });
    }
}