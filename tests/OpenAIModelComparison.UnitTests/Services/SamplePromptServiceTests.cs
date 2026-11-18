using System.Net;
using OpenAIModelComparison.Services;
using OpenAIModelComparison.UnitTests.TestDoubles;

namespace OpenAIModelComparison.UnitTests.Services;

public class SamplePromptServiceTests
{
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

        var prompts = await service.LoadSamplePromptsAsync();

        var prompt = Assert.Single(prompts);
        Assert.Equal("Coding", prompt.Title);
        Assert.Equal(
            "https://localhost/data/sampleprompts.json",
            handler.LastRequest?.RequestUri?.ToString());
    }

    [Fact]
    public async Task LoadSamplePromptsAsync_WithNoBody_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = new(_ =>
            new HttpResponseMessage(HttpStatusCode.NoContent));
        SamplePromptService service = CreateService(handler);

        var prompts = await service.LoadSamplePromptsAsync();

        Assert.Empty(prompts);
    }

    [Fact]
    public async Task LoadSamplePromptsAsync_WithInvalidJson_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson("[ invalid");
        SamplePromptService service = CreateService(handler);

        var prompts = await service.LoadSamplePromptsAsync();

        Assert.Empty(prompts);
    }

    private static SamplePromptService CreateService(TestHttpMessageHandler handler) =>
        new(new HttpClient(handler), new TestNavigationManager());
}
