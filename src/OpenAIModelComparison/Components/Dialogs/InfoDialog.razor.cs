using Microsoft.AspNetCore.Components;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

/// <summary>
/// Dialog component for displaying general information about the application.
/// </summary>
public partial class InfoDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
}
