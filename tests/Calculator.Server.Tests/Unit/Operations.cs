using Calculator.Contracts;
using Calculator.Server.Services;
using Microsoft.Extensions.Logging.Abstractions;
namespace Calculator.Server.Tests.Unit;

[Trait("Category", "Unit")]
public class Operations
{

    [Theory]
    [InlineData(-1, 2, Operation.Addition, 1)]
    [InlineData(1, -2, Operation.Addition, -1)]
    [InlineData(-10, 5, Operation.Subtraction, -15)]
    [InlineData(10, -5, Operation.Subtraction, 15)]
    [InlineData(-4, 3, Operation.Multiplication, -12)]
    [InlineData(4, -3, Operation.Multiplication, -12)]
    [InlineData(-10, 2, Operation.Division, -5)]
    [InlineData(10, -2, Operation.Division, -5)]
    public async Task Calculate_ReturnsExpectedResult(
        double left,
        double right,
        Operation operation,
        double expected)
    {
        // Arrange
        var service = new CalculatorGrpcService(
            NullLogger<CalculatorGrpcService>.Instance);

        var request = new CalculationRequest
        {
            Left = left,
            Right = right,
            Operation = operation
        };

        // Act
        var response = await service.Calculate(request, context: null!);

        // Assert
        Assert.Equal(CalculationResultCode.Ok, response.Code);
        Assert.Equal(expected, response.Value);
    }
    
    [Fact]
    public async Task Calculate_DivisionByZero_ReturnsDivisionByZeroCode()
    {
        // Arrange
        var service = new CalculatorGrpcService(NullLogger<CalculatorGrpcService>.Instance);

        var request = new CalculationRequest
        {
            Left = 10,
            Right = 0,
            Operation = Operation.Division
        };

        // Act
        var response = await service.Calculate(request, context: null!);

        // Assert
        Assert.Equal(CalculationResultCode.DivisionByZero, response.Code);
    }
    
}