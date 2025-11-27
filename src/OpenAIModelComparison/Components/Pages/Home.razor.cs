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

    [Inject]
    private ISamplePromptService SamplePromptService { get; set; } = default!;

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
    private List<SamplePrompt> samplePrompts = [];

    private int? maxTokens = null;
    private float? temperature = 1.0f;
    private float? topP = null;
    private float? frequencyPenalty = null;

    /// <summary>
    /// Initializes the component by loading available models and sample prompts.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await Task.WhenAll(
            LoadModelsAsync(),
            LoadSamplePromptsAsync()
        );
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
    /// Loads the sample prompts from the configuration service.
    /// </summary>
    private async Task LoadSamplePromptsAsync()
    {
        try
        {
            samplePrompts = await SamplePromptService.LoadSamplePromptsAsync();

            if (samplePrompts.Count == 0)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Warning",
                    Detail = "No sample prompts could be loaded from configuration",
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
                Detail = $"Failed to load sample prompts: {ex.Message}",
                Duration = 4000
            });
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
                Width = "800px",
                Height = "auto",
                Style = "max-height: 85vh; margin: 2rem; overflow-y: auto;",
                Resizable = false,
                Draggable = false,
                AutoFocusFirstElement = false
            });
    }
}
