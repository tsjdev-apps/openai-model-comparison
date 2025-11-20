using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Components.Dialogs;
using OpenAIModelComparison.Models;
using OpenAIModelComparison.Services;
using Radzen;

namespace OpenAIModelComparison.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject]
    private NotificationService NotificationService { get; set; } = default!;

    [Inject]
    private ModelConfigurationService ModelConfigurationService { get; set; } = default!;

    [Inject]
    private OpenAIComparisonService OpenAIComparisonService { get; set; } = default!;

    [Inject]
    private DialogService DialogService { get; set; } = default!;

    private string apiKey = string.Empty;
    private string userMessage = string.Empty;
    private bool isProcessing = false;
    private bool isLoadingModels = true;

    private List<OpenAIModel> availableModels = [];
    private List<OpenAIModel> selectedModels => [.. availableModels.Where(m => m.IsSelected)];
    private List<ModelComparisonResult> comparisonResults = [];

    protected override async Task OnInitializedAsync()
    {
        await LoadModelsAsync();
    }

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

    private void ToggleModel(OpenAIModel model)
    {
        model.IsSelected = !model.IsSelected;
        StateHasChanged();
    }

    private void OnModelSelectionChanged(OpenAIModel _)
    {
        StateHasChanged();
    }

    private bool CanSendMessage()
    {
        return !string.IsNullOrWhiteSpace(apiKey)
            && selectedModels.Count != 0
            && !string.IsNullOrWhiteSpace(userMessage)
            && !isProcessing
            && !isLoadingModels;
    }

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
                selectedModels);

            var successCount = comparisonResults.Count(r => r.IsSuccess);
            var failureCount = comparisonResults.Count - successCount;

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

    private async Task ShowResponseDetail(ModelComparisonResult result)
    {
        await DialogService.OpenAsync<ResponseDetailDialog>(
            "Response Details",
            new Dictionary<string, object> { { "Result", result } },
            new DialogOptions { Width = "700px", Height = "600px", Resizable = true, Draggable = true });
    }

    private string GetResponsePreview(string response, int maxLength = 100)
    {
        if (string.IsNullOrEmpty(response))
            return "No response";

        return response.Length <= maxLength 
            ? response 
            : response[..maxLength] + "...";
    }
}
