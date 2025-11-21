using Microsoft.AspNetCore.Components;

namespace OpenAIModelComparison.Components;

/// <summary>
/// Component for managing application routes.
/// </summary>
public partial class Routes : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
}
