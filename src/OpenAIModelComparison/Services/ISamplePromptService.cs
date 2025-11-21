using OpenAIModelComparison.Models;

namespace OpenAIModelComparison.Services;

/// <summary>
/// Defines the contract for loading sample prompts.
/// </summary>
public interface ISamplePromptService
{
    /// <summary>
    /// Loads sample prompts from the configuration file.
    /// </summary>
    /// <returns>A list of sample prompts.</returns>
    Task<List<SamplePrompt>> LoadSamplePromptsAsync();
}
