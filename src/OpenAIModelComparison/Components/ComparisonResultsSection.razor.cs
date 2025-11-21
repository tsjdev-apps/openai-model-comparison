using Microsoft.AspNetCore.Components;
using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Components;

/// <summary>
/// Component for displaying comparison results in a table and card format.
/// </summary>
public partial class ComparisonResultsSection : ComponentBase
{
    /// <summary>
    /// Gets or sets the list of comparison results to display.
    /// </summary>
    [Parameter]
    public required List<ModelComparisonResult> Results { get; set; }
    
    /// <summary>
    /// Event callback invoked when a result is selected for detailed view.
    /// </summary>
    [Parameter]
    public EventCallback<ModelComparisonResult> OnResultSelected { get; set; }
    
    /// <summary>
    /// Gets a preview of the response text, truncated to the specified maximum length.
    /// </summary>
    /// <param name="response">The full response text.</param>
    /// <param name="maxLength">The maximum length of the preview.</param>
    /// <returns>A truncated version of the response or "No response" if empty.</returns>
    private static string GetResponsePreview(string response, int maxLength = 100)
    {
        if (string.IsNullOrEmpty(response))
            return "No response";

        return response.Length <= maxLength 
            ? response 
            : response[..maxLength] + "...";
    }
}
