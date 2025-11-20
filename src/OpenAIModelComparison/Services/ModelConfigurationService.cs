using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

public class ModelConfigurationService
{
    private readonly HttpClient _httpClient;
    private readonly NavigationManager _navigationManager;

    public ModelConfigurationService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _navigationManager = navigationManager;
    }

    public async Task<List<OpenAIModel>> LoadModelsAsync()
    {
        try
        {
            // Use NavigationManager to get the correct base URI
            var baseUri = _navigationManager.BaseUri;
            var modelsUri = new Uri(new Uri(baseUri), "data/models.json");

            var models = await _httpClient.GetFromJsonAsync<List<OpenAIModel>>(modelsUri.ToString());
            return models ?? new List<OpenAIModel>();
        }
        catch (Exception ex)
        {
            // Log error and return empty list
            Console.WriteLine($"Error loading models: {ex.Message}");
            return new List<OpenAIModel>();
        }
    }
}
