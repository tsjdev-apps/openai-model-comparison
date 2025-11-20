using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Service for loading and managing OpenAI model configurations.
/// </summary>
public class ModelConfigurationService : IModelConfigurationService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelConfigurationService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for making requests.</param>
    /// <param name="navigationManager">The navigation manager for resolving URIs.</param>
    public ModelConfigurationService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    /// <summary>
    /// Loads the available OpenAI models from the configuration file.
    /// </summary>
    /// <returns>A list of available OpenAI models, or an empty list if loading fails.</returns>
    public async Task<List<OpenAIModel>> LoadModelsAsync()
    {
        try
        {
            string baseUri = _navigationManager.BaseUri;
            Uri modelsUri = new Uri(new Uri(baseUri), "data/models.json");

            List<OpenAIModel>? models = await _httpClient.GetFromJsonAsync<List<OpenAIModel>>(modelsUri.ToString());
            return models ?? new List<OpenAIModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading models: {ex.Message}");
            return new List<OpenAIModel>();
        }
    }
}
