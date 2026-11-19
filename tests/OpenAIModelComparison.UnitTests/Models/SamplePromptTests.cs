using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.UnitTests.Models;

/// <summary>
/// Tests sample prompt selection behavior.
/// </summary>
public class SamplePromptTests
{
    /// <summary>
    /// Verifies that a category without prompts returns an empty string.
    /// </summary>
    [Fact]
    public void GetRandomPrompt_WithNoPrompts_ReturnsEmptyString()
    {
        SamplePrompt prompt = CreatePrompt([]);

        string result = prompt.GetRandomPrompt();

        Assert.Empty(result);
    }

    /// <summary>
    /// Verifies that a category with one prompt repeatedly returns that prompt.
    /// </summary>
    [Fact]
    public void GetRandomPrompt_WithOnePrompt_ReturnsOnlyPrompt()
    {
        SamplePrompt prompt = CreatePrompt(["Only prompt"]);

        Assert.Equal("Only prompt", prompt.GetRandomPrompt());
        Assert.Equal("Only prompt", prompt.GetRandomPrompt());
    }

    /// <summary>
    /// Verifies that consecutive selections differ when multiple prompts are available.
    /// </summary>
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

    /// <summary>
    /// Creates a sample prompt category with the supplied prompts.
    /// </summary>
    /// <param name="prompts">The prompts available for selection.</param>
    /// <returns>The configured sample prompt category.</returns>
    private static SamplePrompt CreatePrompt(List<string> prompts) =>
        new()
        {
            Title = "Category",
            Description = "Description",
            Prompts = prompts,
            Icon = "psychology"
        };
}
