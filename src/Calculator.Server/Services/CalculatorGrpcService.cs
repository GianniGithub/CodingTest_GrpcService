using Calculator.Contracts;
using Grpc.Core;

namespace Calculator.Server.Services;

public class CalculatorGrpcService(ILogger<CalculatorGrpcService> logger) : CalculatorService.CalculatorServiceBase
{
    public override Task<CalculationResponse> Calculate(
        CalculationRequest request, ServerCallContext context)
    {
        logger.LogInformation(
            "Calculate {Left} {Operation} {Right}",
            request.Left,
            request.Operation,
            request.Right);

        if (request.Operation == Operation.Division && request.Right == 0)
        {
            return Task.FromResult(new CalculationResponse
            {
                Value = double.NaN,
                Code = CalculationResultCode.DivisionByZero
            });
        }

        var value = request.Operation switch
        {
            Operation.Addition => request.Left + request.Right,
            Operation.Subtraction => request.Left - request.Right,
            Operation.Multiplication => request.Left * request.Right,
            Operation.Division => request.Left / request.Right,
            _ => double.NaN
        };

        var code = double.IsNaN(value)
            ? CalculationResultCode.Unspecified
            : CalculationResultCode.Ok;

        return Task.FromResult(new CalculationResponse
        {
            Value = value,
            Code = code
        });
    }
}