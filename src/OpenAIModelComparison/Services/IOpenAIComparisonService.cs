using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Defines the contract for comparing responses from different OpenAI models.
/// </summary>
public interface IOpenAIComparisonService
{
    /// <summary>
    /// Compares multiple OpenAI models by sending the same user message to each model and collecting their responses.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key for authentication.</param>
    /// <param name="userMessage">The message to send to each model.</param>
    /// <param name="selectedModels">The list of models to compare.</param>
    /// <returns>A list of comparison results, one for each model.</returns>
    Task<List<ModelComparisonResult>> CompareModelsAsync(
        string apiKey,
        string userMessage,
        List<OpenAIModel> selectedModels);
}
