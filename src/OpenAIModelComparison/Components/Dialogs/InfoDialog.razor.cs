using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Services;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

/// <summary>
/// Dialog component for displaying general information about the application.
/// </summary>
public partial class InfoDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
    
    [Inject]
    private IModelConfigurationService ModelConfigurationService { get; set; } = default!;
    
    private string ModelVersion => ModelConfigurationService.Version ?? "Unknown";
}
