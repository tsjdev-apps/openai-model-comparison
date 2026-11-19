using Microsoft.AspNetCore.Components;

namespace OpenAIModelComparison.Components.Views;

/// <summary>
/// Component for managing advanced chat completion settings.
/// </summary>
public partial class AdvancedSettingsSection : ComponentBase
{
    /// <summary>
    /// Gets or sets whether the advanced settings section is expanded.
    /// </summary>
    [Parameter]
    public bool IsExpanded { get; set; }
    
    /// <summary>
    /// Gets or sets whether the component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of tokens to generate.
    /// </summary>
    [Parameter]
    public int? MaxTokens { get; set; }
    
    /// <summary>
    /// Event callback invoked when MaxTokens changes.
    /// </summary>
    [Parameter]
    public EventCallback<int?> MaxTokensChanged { get; set; }
    
    /// <summary>
    /// Event callback invoked when the toggle button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnToggle { get; set; }
    
    /// <summary>
    /// Event callback invoked when the reset button is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnReset { get; set; }
    
    /// <summary>
    /// Handles clicking on the header to toggle the advanced settings.
    /// </summary>
    private async Task HandleHeaderClick()
    {
        if (!IsDisabled)
        {
            await OnToggle.InvokeAsync();
        }
    }
    
    /// <summary>
    /// Handles clicking on the reset button.
    /// </summary>
    private async Task HandleResetClick()
    {
        await OnReset.InvokeAsync();
    }

    /// <summary>
    /// Handles clicking on the advanced settings toggle.
    /// </summary>
    private async Task HandleSummaryClick()
    {
        await HandleHeaderClick();
    }
}
