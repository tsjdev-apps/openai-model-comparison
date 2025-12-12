using System.Text.Json.Serialization;

namespace OpenAIModelComparison.Models;

/// <summary>
/// Represents the configuration file structure for OpenAI models.
/// </summary>
public class ModelConfiguration
{
    /// <summary>
    /// Gets or sets the version of the model configuration file.
    /// </summary>
    [JsonPropertyName("version")]
    public required string Version { get; set; }
    
    /// <summary>
    /// Gets or sets the list of available OpenAI models.
    /// </summary>
    [JsonPropertyName("models")]
    public required List<OpenAIModel> Models { get; set; }
}
