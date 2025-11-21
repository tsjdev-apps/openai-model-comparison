using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Components;

/// <summary>
/// Component for displaying and managing model selection.
/// </summary>
public partial class ModelSelectionSection : ComponentBase
{
    /// <summary>
    /// Gets or sets the list of available models.
    /// </summary>
    [Parameter]
    public required List<OpenAIModel> Models { get; set; }
    
    /// <summary>
    /// Gets or sets whether models are currently being loaded.
    /// </summary>
    [Parameter]
    public bool IsLoading { get; set; }
    
    /// <summary>
    /// Gets or sets whether the component is disabled.
    /// </summary>
    [Parameter]
    public bool IsDisabled { get; set; }
    
    /// <summary>
    /// Event callback invoked when a model is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<OpenAIModel> OnModelToggled { get; set; }
    
    /// <summary>
    /// Event callback invoked when a model selection changes.
    /// </summary>
    [Parameter]
    public EventCallback<OpenAIModel> OnModelChanged { get; set; }
}
