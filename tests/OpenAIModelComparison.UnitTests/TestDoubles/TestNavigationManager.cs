using Microsoft.AspNetCore.Components;

namespace OpenAIModelComparison.UnitTests.TestDoubles;

internal sealed class TestNavigationManager : NavigationManager
{
    public TestNavigationManager(string baseUri = "https://localhost/")
    {
        Initialize(baseUri, baseUri);
    }
}
