namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents the chat completion settings for OpenAI API requests.
/// </summary>
public class ChatSettings
{
    /// <summary>
    /// Gets or sets the maximum number of tokens to generate in the completion.
    /// </summary>
    public int? MaxTokens { get; set; }

    /// <summary>
    /// Gets or sets the sampling temperature (0-2). Higher values make output more random.
    /// </summary>
    public float? Temperature { get; set; }

    /// <summary>
    /// Gets or sets the nucleus sampling parameter (0-1). Alternative to temperature.
    /// </summary>
    public float? TopP { get; set; }

    /// <summary>
    /// Gets or sets the frequency penalty (-2.0 to 2.0). Positive values decrease likelihood of repeating tokens.
    /// </summary>
    public float? FrequencyPenalty { get; set; }
}
