using System.Text.Json.Serialization;

namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents an OpenAI model configuration.
/// </summary>
public class OpenAIModel
{
    private const double TokensPerMillion = 1_000_000.0;

    /// <summary>
    /// Gets or sets the internal name of the model used by the OpenAI API.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    /// <summary>
    /// Gets or sets the display name of the model shown in the UI.
    /// </summary>
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the model is selected for comparison.
    /// </summary>
    [JsonPropertyName("isSelected")]
    public bool IsSelected { get; set; }
    
    /// <summary>
    /// Gets or sets the price in USD per 1 million input tokens.
    /// </summary>
    [JsonPropertyName("inputPricePerMillionTokens")]
    public double InputPricePerMillionTokens { get; set; }
    
    /// <summary>
    /// Gets or sets the price in USD per 1 million output tokens.
    /// </summary>
    [JsonPropertyName("outputPricePerMillionTokens")]
    public double OutputPricePerMillionTokens { get; set; }

    /// <summary>
    /// Calculates the estimated request cost from the configured token prices.
    /// </summary>
    public double CalculateEstimatedCost(int inputTokens, int outputTokens)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(inputTokens);
        ArgumentOutOfRangeException.ThrowIfNegative(outputTokens);

        return inputTokens / TokensPerMillion * InputPricePerMillionTokens
            + outputTokens / TokensPerMillion * OutputPricePerMillionTokens;
    }
}
