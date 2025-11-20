using Microsoft.AspNetCore.Components;

namespace OpenAIModelComparison.Components;

public partial class Routes : ComponentBase
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;
}
