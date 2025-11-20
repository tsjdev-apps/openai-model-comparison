using Microsoft.AspNetCore.Components;
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

    private string apiKey = string.Empty;
    private string userMessage = string.Empty;
    private bool isProcessing = false;
    private bool isLoadingModels = true;

    private List<OpenAIModel> _availableModels = [];
    private List<OpenAIModel> SelectedModels => [.. _availableModels.Where(m => m.IsSelected)];

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
            _availableModels = await ModelConfigurationService.LoadModelsAsync();
            
            if (_availableModels.Count == 0)
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
            && SelectedModels.Count != 0
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
        StateHasChanged();

        try
        {
            // TODO: Implement OpenAI API calls
            await Task.Delay(2000); // Placeholder for actual API calls

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Success,
                Summary = "Success",
                Detail = $"Messages sent to {SelectedModels.Count} model(s)",
                Duration = 4000
            });
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
}
