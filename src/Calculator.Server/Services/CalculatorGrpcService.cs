using Calculator.Contracts;
using Grpc.Core;

namespace Calculator.Server.Services;

public class CalculatorGrpcService(ILogger<CalculatorGrpcService> logger) : CalculatorService.CalculatorServiceBase
{

    public override Task<CalculationResponse> Add(
        BinaryOperationRequest request, ServerCallContext context)
    {
        logger.LogInformation("Add {Left} + {Right}", request.Left, request.Right);
        return Task.FromResult(new CalculationResponse
        {
            Value = request.Left + request.Right
        });
    }
}