using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Components;

/// <summary>
/// Component for displaying sample prompts that users can select.
/// </summary>
public partial class SamplePromptsSection : ComponentBase
{
    /// <summary>
    /// Gets or sets the list of sample prompts to display.
    /// </summary>
    [Parameter]
    public required List<SamplePrompt> SamplePrompts { get; set; }
    
    /// <summary>
    /// Gets or sets whether the component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }
    
    /// <summary>
    /// Event callback invoked when a sample prompt is selected.
    /// </summary>
    [Parameter]
    public EventCallback<SamplePrompt> OnSampleSelected { get; set; }
}
