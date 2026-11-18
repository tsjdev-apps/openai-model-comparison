using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.UnitTests.Models;

public class OpenAIModelTests
{
    [Fact]
    public void CalculateEstimatedCost_UsesConfiguredPrices()
    {
        OpenAIModel model = CreateModel(inputPrice: 2.5, outputPrice: 10);

        double cost = model.CalculateEstimatedCost(inputTokens: 1_500_000, outputTokens: 250_000);

        Assert.Equal(6.25, cost, precision: 10);
    }

    [Theory]
    [InlineData(-1, 0, "inputTokens")]
    [InlineData(0, -1, "outputTokens")]
    public void CalculateEstimatedCost_RejectsNegativeTokenCounts(
        int inputTokens,
        int outputTokens,
        string parameterName)
    {
        OpenAIModel model = CreateModel();

        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => model.CalculateEstimatedCost(inputTokens, outputTokens));

        Assert.Equal(parameterName, exception.ParamName);
    }

    [Fact]
    public void CalculateEstimatedCost_WithNoUsage_ReturnsZero()
    {
        OpenAIModel model = CreateModel();

        double cost = model.CalculateEstimatedCost(0, 0);

        Assert.Equal(0, cost);
    }

    private static OpenAIModel CreateModel(double inputPrice = 0, double outputPrice = 0) =>
        new()
        {
            Name = "test-model",
            DisplayName = "Test model",
            InputPricePerMillionTokens = inputPrice,
            OutputPricePerMillionTokens = outputPrice
        };
}
