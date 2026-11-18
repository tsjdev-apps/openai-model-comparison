using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.UnitTests.Models;

public class SamplePromptTests
{
    [Fact]
    public void GetRandomPrompt_WithNoPrompts_ReturnsEmptyString()
    {
        SamplePrompt prompt = CreatePrompt([]);

        string result = prompt.GetRandomPrompt();

        Assert.Empty(result);
    }

    [Fact]
    public void GetRandomPrompt_WithOnePrompt_ReturnsOnlyPrompt()
    {
        SamplePrompt prompt = CreatePrompt(["Only prompt"]);

        Assert.Equal("Only prompt", prompt.GetRandomPrompt());
        Assert.Equal("Only prompt", prompt.GetRandomPrompt());
    }

    [Fact]
    public void GetRandomPrompt_WithMultiplePrompts_AvoidsConsecutiveDuplicates()
    {
        SamplePrompt prompt = CreatePrompt(["First", "Second"]);

        string previous = prompt.GetRandomPrompt();

        for (int iteration = 0; iteration < 20; iteration++)
        {
            string current = prompt.GetRandomPrompt();
            Assert.NotEqual(previous, current);
            previous = current;
        }
    }

    private static SamplePrompt CreatePrompt(List<string> prompts) =>
        new()
        {
            Title = "Category",
            Description = "Description",
            Prompts = prompts,
            Icon = "psychology"
        };
}
