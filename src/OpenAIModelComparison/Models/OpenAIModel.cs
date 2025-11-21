using System.Text.Json.Serialization;

namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents an OpenAI model configuration.
/// </summary>
public class OpenAIModel
{
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
}
