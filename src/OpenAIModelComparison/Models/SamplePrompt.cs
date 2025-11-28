namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents a sample prompt category that can be used for model comparison.
/// </summary>
public class SamplePrompt
{
    private int _lastUsedIndex = -1;
    
    /// <summary>
    /// Gets or sets the title of the sample prompt category.
    /// </summary>
    public required string Title { get; set; }
    
    /// <summary>
    /// Gets or sets the description of what this prompt category tests.
    /// </summary>
    public required string Description { get; set; }
    
    /// <summary>
    /// Gets or sets the list of possible prompts for this category.
    /// One will be randomly selected when the prompt is used.
    /// </summary>
    public required List<string> Prompts { get; set; }
    
    /// <summary>
    /// Gets or sets the icon name for the prompt card.
    /// </summary>
    public required string Icon { get; set; }
    
    /// <summary>
    /// Gets a randomly selected prompt from the available prompts.
    /// Ensures that consecutive calls return different prompts when possible.
    /// </summary>
    public string GetRandomPrompt()
    {
        if (Prompts == null || Prompts.Count == 0)
        {
            return string.Empty;
        }
        
        if (Prompts.Count == 1)
        {
            return Prompts[0];
        }
        
        int index;
        do
        {
            index = Random.Shared.Next(Prompts.Count);
        }
        while (index == _lastUsedIndex);
        
        _lastUsedIndex = index;
        return Prompts[index];
    }
}
