using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Defines the contract for loading and managing OpenAI model configurations.
/// </summary>
public interface IModelConfigurationService
{
    /// <summary>
    /// Loads the available OpenAI models from the configuration file.
    /// </summary>
    /// <returns>A list of available OpenAI models.</returns>
    Task<List<OpenAIModel>> LoadModelsAsync();
}
