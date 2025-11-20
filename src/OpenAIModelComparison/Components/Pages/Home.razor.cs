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

    private List<OpenAIModel> availableModels = [];
    private List<OpenAIModel> SelectedModels => [.. availableModels.Where(m => m.IsSelected)];
    private List<ModelComparisonResult> comparisonResults = [];

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
        model.IsSelected = !model.IsSelected;
        StateHasChanged();
    }

    /// <summary>
    /// Handles the model selection change event.
    /// </summary>
    /// <param name="_">The model that was changed (unused).</param>
    private void OnModelSelectionChanged(OpenAIModel _)
    {
        StateHasChanged();
    }

    /// <summary>
    /// Determines whether a message can be sent based on current state.
    /// </summary>
    /// <returns>True if a message can be sent, false otherwise.</returns>
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
        StateHasChanged();

        try
        {
            comparisonResults = await OpenAIComparisonService.CompareModelsAsync(
                apiKey,
                userMessage,
                SelectedModels);

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
            new DialogOptions { Width = "700px", Height = "600px", Resizable = true, Draggable = true });
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
}
