using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Service for loading and managing OpenAI model configurations.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ModelConfigurationService"/> class.
/// </remarks>
/// <param name="httpClient">The HTTP client for making requests.</param>
/// <param name="navigationManager">The navigation manager for resolving URIs.</param>
public class ModelConfigurationService(
    HttpClient httpClient, 
    NavigationManager navigationManager) : IModelConfigurationService
{

    /// <summary>
    /// Loads the available OpenAI models from the configuration file.
    /// </summary>
    /// <returns>A list of available OpenAI models, or an empty list if loading fails.</returns>
    public async Task<List<OpenAIModel>> LoadModelsAsync()
    {
        try
        {
            string baseUri = navigationManager.BaseUri;
            Uri modelsUri = new(new Uri(baseUri), "data/models.json");

            List<OpenAIModel>? models = await httpClient.GetFromJsonAsync<List<OpenAIModel>>(modelsUri.ToString());
            return models ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading models: {ex.Message}");
            return [];
        }
    }
}
