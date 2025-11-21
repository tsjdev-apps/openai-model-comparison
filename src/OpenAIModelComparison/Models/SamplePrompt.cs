namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents a sample prompt that can be used for model comparison.
/// </summary>
public class SamplePrompt
{
    /// <summary>
    /// Gets or sets the title of the sample prompt.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the description of what this prompt tests.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the actual prompt text.
    /// </summary>
    public required string Prompt { get; set; }
    
    /// <summary>
    /// Gets or sets the icon name for the prompt card.
    /// </summary>
    public required string Icon { get; set; }
}
