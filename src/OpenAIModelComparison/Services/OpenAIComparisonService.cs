using OpenAI;
using OpenAI.Chat;
using OpenAIModelComparison.Models;
using System.Diagnostics;

namespace OpenAIModelComparison.Services;

public class OpenAIComparisonService
{
    private readonly SemaphoreSlim _throttler = new(1, 1); // Allow only 1 concurrent request
    private readonly TimeSpan _delayBetweenRequests = TimeSpan.FromSeconds(1); // Delay between requests

    public async Task<List<ModelComparisonResult>> CompareModelsAsync(
        string apiKey,
        string userMessage,
        List<OpenAIModel> selectedModels)
    {
        var results = new List<ModelComparisonResult>();

        // Process models sequentially with throttling
        foreach (var model in selectedModels)
        {
            var result = await GetModelResponseAsync(apiKey, userMessage, model);
            results.Add(result);
            
            // Add delay between requests (except for the last one)
            if (model != selectedModels[^1])
            {
                await Task.Delay(_delayBetweenRequests);
            }
        }

        return results;
    }

    private async Task<ModelComparisonResult> GetModelResponseAsync(
        string apiKey,
        string userMessage,
        OpenAIModel model)
    {
        await _throttler.WaitAsync();
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            var client = new OpenAIClient(apiKey);
            var chatClient = client.GetChatClient(model.Name);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a helpful assistant."),
                new UserChatMessage(userMessage)
            };

            var response = await chatClient.CompleteChatAsync(messages);
            stopwatch.Stop();

            var completion = response.Value;
            var content = completion.Content[0].Text;

            return new ModelComparisonResult
            {
                ModelName = model.Name,
                ModelDisplayName = model.DisplayName,
                Response = content,
                InputTokens = completion.Usage.InputTokenCount,
                OutputTokens = completion.Usage.OutputTokenCount,
                TotalTokens = completion.Usage.TotalTokenCount,
                DurationInSeconds = stopwatch.Elapsed.TotalSeconds,
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