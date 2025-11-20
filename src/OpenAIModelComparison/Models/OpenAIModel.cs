using System.Text.Json.Serialization;

namespace OpenAIModelComparison.Models;

public class OpenAIModel
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("displayName")]
    public required string DisplayName { get; set; }
    
    [JsonPropertyName("isSelected")]
    public bool IsSelected { get; set; }
}
