using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.UnitTests.Models;

/// <summary>
/// Tests the cost calculation behavior of <see cref="OpenAIModel"/>.
/// </summary>
public class OpenAIModelTests
{
    /// <summary>
    /// Verifies that configured input and output prices are applied to token usage.
    /// </summary>
    [Fact]
    public void CalculateEstimatedCost_UsesConfiguredPrices()
    {
        OpenAIModel model = CreateModel(inputPrice: 2.5, outputPrice: 10);

        double cost = model.CalculateEstimatedCost(inputTokens: 1_500_000, outputTokens: 250_000);

        Assert.Equal(6.25, cost, precision: 10);
    }

    /// <summary>
    /// Verifies that negative token counts are rejected.
    /// </summary>
    /// <param name="inputTokens">The input token count to test.</param>
    /// <param name="outputTokens">The output token count to test.</param>
    /// <param name="parameterName">The expected invalid parameter name.</param>
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

    /// <summary>
    /// Verifies that a request without token usage has no estimated cost.
    /// </summary>
    [Fact]
    public void CalculateEstimatedCost_WithNoUsage_ReturnsZero()
    {
        OpenAIModel model = CreateModel();

        double cost = model.CalculateEstimatedCost(0, 0);

        Assert.Equal(0, cost);
    }

    /// <summary>
    /// Creates an OpenAI model configured with the supplied token prices.
    /// </summary>
    /// <param name="inputPrice">The price per million input tokens.</param>
    /// <param name="outputPrice">The price per million output tokens.</param>
    /// <returns>The configured model.</returns>
    private static OpenAIModel CreateModel(double inputPrice = 0, double outputPrice = 0) =>
        new()
        {
            Name = "test-model",
            DisplayName = "Test model",
            InputPricePerMillionTokens = inputPrice,
            OutputPricePerMillionTokens = outputPrice
        };
}
