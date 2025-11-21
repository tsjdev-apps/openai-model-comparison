using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Service for loading sample prompts from a JSON configuration file.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="SamplePromptService"/> class.
/// </remarks>
/// <param name="httpClient">The HTTP client for loading the configuration file.</param>
public class SamplePromptService(HttpClient httpClient, NavigationManager navigationManager) : ISamplePromptService
{
    /// <summary>
    /// Loads sample prompts from the JSON configuration file.
    /// </summary>
    /// <returns>A list of sample prompts, or an empty list if loading fails.</returns>
    public async Task<List<SamplePrompt>> LoadSamplePromptsAsync()
    {
        try
        {
            string baseUri = navigationManager.BaseUri;
            Uri samplePromptsUri = new(new Uri(baseUri), "data/sampleprompts.json");

            List<SamplePrompt>? prompts = await httpClient.GetFromJsonAsync<List<SamplePrompt>>(samplePromptsUri.ToString());
            return prompts ?? [];
        }
        catch
        {
            return [];
        }
    }
}
