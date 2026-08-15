using Calculator.Contracts;
using Calculator.Server.Services;
using Microsoft.Extensions.Logging.Abstractions;
namespace Calculator.Server.Tests;

public class Addition
{
    [Theory]
    [InlineData(5, 7, 12)]
    [InlineData(-5, 7, 2)]
    [InlineData(0, 0, 0)]
    [InlineData(-3, -4, -7)]
    public async Task Add_ReturnsSumOfLeftAndRight(double left, double right, double expected)
    {
        // Arrange
        var service = new CalculatorGrpcService(
            NullLogger<CalculatorGrpcService>.Instance);

        var request = new BinaryOperationRequest
        {
            Left = left,
            Right = right
        };

        // Act
        var response = await service.Add(request, context: null!);

        // Assert
        Assert.Equal(expected, response.Value);
    }
    
}