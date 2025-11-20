using Microsoft.AspNetCore.Components;
using Radzen;

namespace OpenAIModelComparison.Components.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;

    private async Task ShowInfoDialog()
    {
        await DialogService.OpenAsync<Dialogs.InfoDialog>("More Information",
            null,
            new DialogOptions() { Width = "500px", CloseDialogOnOverlayClick = true });
    }

    private async Task ShowImprintDialog()
    {
        await DialogService.OpenAsync<Dialogs.ImprintDialog>("Imprint",
            null,
            new DialogOptions() { Width = "600px", CloseDialogOnOverlayClick = true });
    }
}
