using Microsoft.AspNetCore.Components;
using Radzen;

namespace OpenAIModelComparison.Components.Layout;

/// <summary>
/// Main layout component for the application.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
    [Inject]
    private DialogService DialogService { get; set; } = default!;

    /// <summary>
    /// Opens a dialog displaying additional information about the application.
    /// </summary>
    private async Task ShowInfoDialog()
    {
        await DialogService.OpenAsync<Dialogs.InfoDialog>("More Information",
            null,
            new DialogOptions() 
            { 
                Width = "600px",
                Height = "auto",
                Style = "max-height: 85vh; margin: 2rem;",
                CloseDialogOnOverlayClick = true,
                AutoFocusFirstElement = false,
                Resizable = false,
                Draggable = false
            });
    }

    /// <summary>
    /// Opens a dialog displaying imprint and legal information.
    /// </summary>
    private async Task ShowImprintDialog()
    {
        await DialogService.OpenAsync<Dialogs.ImprintDialog>("Imprint",
            null,
            new DialogOptions() 
            { 
                Width = "750px",
                Height = "auto",
                Style = "max-height: 85vh; margin: 2rem; overflow-y: auto;",
                CloseDialogOnOverlayClick = true,
                AutoFocusFirstElement = false,
                Resizable = false,
                Draggable = false
            });
    }
}
