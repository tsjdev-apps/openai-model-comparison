using Microsoft.AspNetCore.Components;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

/// <summary>
/// Dialog component for displaying imprint and legal information.
/// </summary>
public partial class ImprintDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
}
