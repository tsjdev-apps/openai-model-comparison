namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents the result of a model comparison request.
/// </summary>
public class ModelComparisonResult
{
    /// <summary>
    /// Gets or sets the internal name of the model.
    /// </summary>
    public required string ModelName { get; set; }
    
    /// <summary>
    /// Gets or sets the display name of the model.
    /// </summary>
    public required string ModelDisplayName { get; set; }
    
    /// <summary>
    /// Gets or sets the response text from the model.
    /// </summary>
    public required string Response { get; set; }
    
    /// <summary>
    /// Gets or sets the number of input tokens used.
    /// </summary>
    public int InputTokens { get; set; }
    
    /// <summary>
    /// Gets or sets the number of output tokens generated.
    /// </summary>
    public int OutputTokens { get; set; }
    
    /// <summary>
    /// Gets or sets the total number of tokens used.
    /// </summary>
    public int TotalTokens { get; set; }
    
    /// <summary>
    /// Gets or sets the duration of the request in seconds.
    /// </summary>
    public double DurationInSeconds { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the request was successful.
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// Gets or sets the error message if the request failed.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
