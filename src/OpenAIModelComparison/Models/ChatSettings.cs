namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents the Responses API settings for OpenAI requests.
/// </summary>
public class ChatSettings
{
    /// <summary>
    /// Gets or sets the maximum number of output and reasoning tokens to generate.
    /// </summary>
    public int? MaxTokens { get; set; }
}
