using OpenAI.Responses;
using OpenAIModelComparison.Models;
using System.Diagnostics;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Service for comparing responses from different OpenAI models.
/// </summary>
public class OpenAIComparisonService : IOpenAIComparisonService
{
    private readonly SemaphoreSlim _throttler = new(1, 1);
    private readonly TimeSpan _delayBetweenRequests = TimeSpan.FromSeconds(1);

    /// <summary>
    /// Compares multiple OpenAI models by sending the same user message to each model and collecting their responses.
    /// Models are processed sequentially with throttling to respect API rate limits.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key for authentication.</param>
    /// <param name="userMessage">The message to send to each model.</param>
    /// <param name="selectedModels">The list of models to compare.</param>
    /// <param name="settings">Optional chat settings to customize the API request.</param>
    /// <returns>A list of comparison results, one for each model.</returns>
    public async Task<List<ModelComparisonResult>> CompareModelsAsync(
        string apiKey,
        string userMessage,
        List<OpenAIModel> selectedModels,
        ChatSettings? settings = null)
    {
        List<ModelComparisonResult> results = [];

        foreach (OpenAIModel model in selectedModels)
        {
            ModelComparisonResult result = await GetModelResponseAsync(apiKey, userMessage, model, settings);
            results.Add(result);
            
            if (model != selectedModels[^1])
            {
                await Task.Delay(_delayBetweenRequests);
            }
        }

        return results;
    }

    /// <summary>
    /// Compares a single OpenAI model by sending a user message and collecting the response.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key for authentication.</param>
    /// <param name="userMessage">The message to send to the model.</param>
    /// <param name="model">The model to query.</param>
    /// <param name="settings">Optional chat settings to customize the API request.</param>
    /// <returns>A comparison result for the model.</returns>
    public async Task<ModelComparisonResult> CompareModelAsync(
        string apiKey,
        string userMessage,
        OpenAIModel model,
        ChatSettings? settings = null)
    {
        return await GetModelResponseAsync(apiKey, userMessage, model, settings);
    }

    /// <summary>
    /// Gets a response from a specific OpenAI model for the given user message.
    /// </summary>
    /// <param name="apiKey">The OpenAI API key for authentication.</param>
    /// <param name="userMessage">The message to send to the model.</param>
    /// <param name="model">The model to query.</param>
    /// <param name="settings">Optional chat settings to customize the API request.</param>
    /// <returns>A comparison result containing the model's response and metrics.</returns>
    private async Task<ModelComparisonResult> GetModelResponseAsync(
        string apiKey,
        string userMessage,
        OpenAIModel model,
        ChatSettings? settings)
    {
        await _throttler.WaitAsync();
        Stopwatch stopwatch = Stopwatch.StartNew();
        
        try
        {
            ResponsesClient client = new(apiKey);
            CreateResponseOptions options = new()
            {
                Model = model.Name,
                MaxOutputTokenCount = settings?.MaxTokens
            };
            options.InputItems.Add(ResponseItem.CreateUserMessageItem(userMessage));

            ResponseResult response = await client.CreateResponseAsync(options);
            stopwatch.Stop();

            string content = response.GetOutputText();
            
            int inputTokens = response.Usage.InputTokenCount;
            int outputTokens = response.Usage.OutputTokenCount;
            
            double totalCost = model.CalculateEstimatedCost(inputTokens, outputTokens);

            return new ModelComparisonResult
            {
                ModelName = model.Name,
                ModelDisplayName = model.DisplayName,
                Response = content,
                InputTokens = inputTokens,
                OutputTokens = outputTokens,
                TotalTokens = response.Usage.TotalTokenCount,
                DurationInSeconds = stopwatch.Elapsed.TotalSeconds,
                EstimatedCost = totalCost,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            
            return new ModelComparisonResult
            {
                ModelName = model.Name,
                ModelDisplayName = model.DisplayName,
                Response = string.Empty,
                InputTokens = 0,
                OutputTokens = 0,
                TotalTokens = 0,
                DurationInSeconds = stopwatch.Elapsed.TotalSeconds,
                EstimatedCost = 0,
                IsSuccess = false,
                ErrorMessage = ex.Message
            };
        }
        finally
        {
            _throttler.Release();
        }
    }
}
