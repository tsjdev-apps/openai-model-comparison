using System.Net;
using OpenAIModelComparison.Models;
using OpenAIModelComparison.Services;
using OpenAIModelComparison.UnitTests.TestDoubles;

namespace OpenAIModelComparison.UnitTests.Services;

/// <summary>
/// Tests loading sample prompt configuration data.
/// </summary>
public class SamplePromptServiceTests
{
    /// <summary>
    /// Verifies that prompts are loaded from the expected endpoint.
    /// </summary>
    [Fact]
    public async Task LoadSamplePromptsAsync_LoadsPromptsFromExpectedEndpoint()
    {
        const string json = """
            [
              {
                "title": "Coding",
                "description": "Test coding",
                "prompts": ["Write a function"],
                "icon": "code"
              }
            ]
            """;
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson(json);
        SamplePromptService service = CreateService(handler);

        List<SamplePrompt> prompts = await service.LoadSamplePromptsAsync();

        SamplePrompt prompt = Assert.Single(prompts);
        Assert.Equal("Coding", prompt.Title);
        Assert.Equal(
            "https://localhost/data/sampleprompts.json",
            handler.LastRequest?.RequestUri?.ToString());
    }

    /// <summary>
    /// Verifies that a response without a body produces an empty prompt list.
    /// </summary>
    [Fact]
    public async Task LoadSamplePromptsAsync_WithNoBody_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = new(_ =>
            new HttpResponseMessage(HttpStatusCode.NoContent));
        SamplePromptService service = CreateService(handler);

        List<SamplePrompt> prompts = await service.LoadSamplePromptsAsync();

        Assert.Empty(prompts);
    }

    /// <summary>
    /// Verifies that invalid JSON produces an empty prompt list.
    /// </summary>
    [Fact]
    public async Task LoadSamplePromptsAsync_WithInvalidJson_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson("[ invalid");
        SamplePromptService service = CreateService(handler);

        List<SamplePrompt> prompts = await service.LoadSamplePromptsAsync();

        Assert.Empty(prompts);
    }

    /// <summary>
    /// Creates a sample prompt service using the supplied handler.
    /// </summary>
    /// <param name="handler">The handler that provides HTTP responses.</param>
    /// <returns>The configured service.</returns>
    private static SamplePromptService CreateService(TestHttpMessageHandler handler) =>
        new(new HttpClient(handler), new TestNavigationManager());
}
