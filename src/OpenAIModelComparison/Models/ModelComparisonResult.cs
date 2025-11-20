namespace OpenAIModelComparison.Models;

public class ModelComparisonResult
{
    public required string ModelName { get; set; }
    public required string ModelDisplayName { get; set; }
    public required string Response { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public int TotalTokens { get; set; }
    public double DurationInSeconds { get; set; }
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
}
