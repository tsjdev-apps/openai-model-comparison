using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

/// <summary>
/// Dialog component for displaying detailed information about a model comparison result.
/// </summary>
public partial class ResponseDetailDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Gets or sets the model comparison result to display.
    /// </summary>
    [Parameter]
    public required ModelComparisonResult Result { get; set; }
}
