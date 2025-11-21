using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Components.Dialogs;
using OpenAIModelComparison.Models;
using OpenAIModelComparison.Services;
using Radzen;

namespace OpenAIModelComparison.Components.Pages;

/// <summary>
/// Home page component for the OpenAI Model Comparison application.
/// </summary>
public partial class Home : ComponentBase
{
    [Inject]
    private NotificationService NotificationService { get; set; } = default!;

    [Inject]
    private IModelConfigurationService ModelConfigurationService { get; set; } = default!;

    [Inject]
    private IOpenAIComparisonService OpenAIComparisonService { get; set; } = default!;

    [Inject]
    private DialogService DialogService { get; set; } = default!;

    private string apiKey = string.Empty;
    private string userMessage = string.Empty;
    private bool isProcessing = false;
    private bool isLoadingModels = true;
    private bool showAdvancedSettings = false;
    private int currentModelIndex = 0;
    private int totalModels = 0;

    private List<OpenAIModel> availableModels = [];
    private List<OpenAIModel> SelectedModels => [.. availableModels.Where(m => m.IsSelected)];
    private List<ModelComparisonResult> comparisonResults = [];

    private readonly List<SamplePrompt> samplePrompts =
    [
        new SamplePrompt
        {
            Title = "Creative Writing",
            Description = "Test creative and narrative abilities",
            Icon = "auto_stories",
            Prompt = "Write a short story (100 words) about a time traveler who accidentally changes a small detail in the past, only to discover it has massive consequences in the present."
        },
        new SamplePrompt
        {
            Title = "Code Generation",
            Description = "Test programming and technical skills",
            Icon = "code",
            Prompt = "Write a C# function that finds the longest palindromic substring in a given string. Include error handling and XML documentation comments."
        },
        new SamplePrompt
        {
            Title = "Problem Solving",
            Description = "Test logical reasoning and analysis",
            Icon = "psychology",
            Prompt = "You have 8 balls. One of them is slightly heavier than the others. Using a balance scale only twice, how can you identify the heavier ball? Explain your reasoning step by step."
        },
        new SamplePrompt
        {
            Title = "Data Analysis",
            Description = "Test analytical and summarization skills",
            Icon = "analytics",
            Prompt = "Analyze the following scenario: A company's website traffic increased by 300% in Q1, but sales only increased by 15%. What could be the potential reasons for this discrepancy? Provide 5 possible explanations with brief reasoning."
        },
        new SamplePrompt
        {
            Title = "Translation & Culture",
            Description = "Test language and cultural understanding",
            Icon = "translate",
            Prompt = "Translate the English idiom 'It's raining cats and dogs' into German, French, and Spanish. Then explain the literal meaning and provide the equivalent idiom in each language if one exists."
        },
        new SamplePrompt
        {
            Title = "Math & Logic",
            Description = "Test mathematical reasoning and explanation",
            Icon = "calculate",
            Prompt = "Explain the Monty Hall problem: You're on a game show with 3 doors. Behind one is a car, behind the others are goats. You pick door 1. The host, who knows what's behind each door, opens door 3 revealing a goat. Should you switch to door 2? Explain with probability calculations."
        }
    ];

    private int? maxTokens = null;
    private float? temperature = 1.0f;
    private float? topP = null;
    private float? frequencyPenalty = null;

    /// <summary>
    /// Initializes the component by loading available models.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await LoadModelsAsync();
    }

    /// <summary>
    /// Loads the available OpenAI models from the configuration service.
    /// </summary>
    private async Task LoadModelsAsync()
    {
        isLoadingModels = true;
        StateHasChanged();

        try
        {
            availableModels = await ModelConfigurationService.LoadModelsAsync();
            
            if (availableModels.Count == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Warning",
                    Detail = "No models could be loaded from configuration",
                    Duration = 4000
                });
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = $"Failed to load models: {ex.Message}",
                Duration = 4000
            });
        }
        finally
        {
            isLoadingModels = false;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Toggles the selection state of a model.
    /// </summary>
    /// <param name="model">The model to toggle.</param>
    private void ToggleModel(OpenAIModel model)
    {
        if (isProcessing || isLoadingModels)
        {
            return;
        }
        
        model.IsSelected = !model.IsSelected;
        StateHasChanged();
    }

    /// <summary>
    /// Handles the model selection change event.
    /// </summary>
    /// <param name="_">The model that was changed (unused).</param>
    private void OnModelSelectionChanged(OpenAIModel _)
    {
        if (isProcessing || isLoadingModels)
        {
            return;
        }
        
        StateHasChanged();
    }

    /// <summary>
    /// Toggles the visibility of advanced settings.
    /// </summary>
    private void ToggleAdvancedSettings()
    {
        if (isProcessing || isLoadingModels)
        {
            return;
        }
        
        showAdvancedSettings = !showAdvancedSettings;
    }

    /// <summary>
    /// Resets all advanced settings to their default values.
    /// </summary>
    private void ResetAdvancedSettings()
    {
        if (isProcessing || isLoadingModels)
        {
            return;
        }
        
        maxTokens = null;
        temperature = 1.0f;
        topP = null;
        frequencyPenalty = null;
        StateHasChanged();
    }

    /// <summary>
    /// Determines whether a message can be sent based on current state.
    /// /// <returns>True if a message can be sent, false otherwise.</returns>
    private bool CanSendMessage()
    {
        return !string.IsNullOrWhiteSpace(apiKey)
            && SelectedModels.Count != 0
            && !string.IsNullOrWhiteSpace(userMessage)
            && !isProcessing
            && !isLoadingModels;
    }

    /// <summary>
    /// Sends the user message to all selected models and displays the results.
    /// </summary>
    private async Task SendMessage()
    {
        if (!CanSendMessage())
        {
            return;
        }

        isProcessing = true;
        comparisonResults.Clear();
        currentModelIndex = 0;
        totalModels = SelectedModels.Count;
        StateHasChanged();

        try
        {
            ChatSettings settings = new()
            {
                MaxTokens = maxTokens,
                Temperature = temperature,
                TopP = topP,
                FrequencyPenalty = frequencyPenalty
            };

            List<ModelComparisonResult> results = [];
            
            foreach (OpenAIModel model in SelectedModels)
            {
                currentModelIndex++;
                StateHasChanged();
                
                ModelComparisonResult result = await OpenAIComparisonService.CompareModelAsync(
                    apiKey,
                    userMessage,
                    model,
                    settings);
                    
                results.Add(result);
                
                if (currentModelIndex < totalModels)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            
            comparisonResults = results;

            int successCount = comparisonResults.Count(r => r.IsSuccess);
            int failureCount = comparisonResults.Count - successCount;

            if (failureCount > 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Partial Success",
                    Detail = $"{successCount} model(s) succeeded, {failureCount} model(s) failed",
                    Duration = 5000
                });
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Success",
                    Detail = $"Responses received from all {successCount} model(s)",
                    Duration = 4000
                });
            }
        }
        catch (Exception ex)
        {
            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Error,
                Summary = "Error",
                Detail = $"An error occurred: {ex.Message}",
                Duration = 4000
            });
        }
        finally
        {
            isProcessing = false;
            currentModelIndex = 0;
            totalModels = 0;
            StateHasChanged();
        }
    }

    /// <summary>
    /// Opens a dialog to show detailed response information for a specific model result.
    /// </summary>
    /// <param name="result">The model comparison result to display.</param>
    private async Task ShowResponseDetail(ModelComparisonResult result)
    {
        await DialogService.OpenAsync<ResponseDetailDialog>(
            "Response Details",
            new Dictionary<string, object> { { "Result", result } },
            new DialogOptions 
            { 
                Width = "700px", 
                Resizable = true, 
                Draggable = true,
                AutoFocusFirstElement = false
            });
    }

    /// <summary>
    /// Gets a preview of the response text, truncated to the specified maximum length.
    /// </summary>
    /// <param name="response">The full response text.</param>
    /// <param name="maxLength">The maximum length of the preview.</param>
    /// <returns>A truncated version of the response or "No response" if empty.</returns>
    private static string GetResponsePreview(string response, int maxLength = 100)
    {
        if (string.IsNullOrEmpty(response))
            return "No response";

        return response.Length <= maxLength 
            ? response 
            : response[..maxLength] + "...";
    }

    /// <summary>
    /// Applies a sample prompt to the message input field.
    /// </summary>
    /// <param name="prompt">The sample prompt to apply.</param>
    private void ApplySamplePrompt(SamplePrompt prompt)
    {
        if (isProcessing || isLoadingModels)
        {
            return;
        }
        
        userMessage = prompt.Prompt;
        StateHasChanged();
    }
}
