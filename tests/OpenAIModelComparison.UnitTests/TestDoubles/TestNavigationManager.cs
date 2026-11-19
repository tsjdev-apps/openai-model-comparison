using Microsoft.AspNetCore.Components;

namespace OpenAIModelComparison.UnitTests.TestDoubles;

/// <summary>
/// Provides a deterministic navigation context for service tests.
/// </summary>
internal sealed class TestNavigationManager : NavigationManager
{
    /// <summary>
    /// Initializes a new navigation manager with the supplied base URI.
    /// </summary>
    /// <param name="baseUri">The base URI used for relative resource resolution.</param>
    public TestNavigationManager(string baseUri = "https://localhost/")
    {
        Initialize(baseUri, baseUri);
    }
}
