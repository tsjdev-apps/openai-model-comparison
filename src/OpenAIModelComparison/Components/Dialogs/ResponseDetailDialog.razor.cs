using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

public partial class ResponseDetailDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;

    [Parameter]
    public required ModelComparisonResult Result { get; set; }
}
