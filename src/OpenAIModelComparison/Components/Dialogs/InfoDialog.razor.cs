using Microsoft.AspNetCore.Components;
using Radzen;

namespace OpenAIModelComparison.Components.Dialogs;

public partial class InfoDialog : ComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;
}
