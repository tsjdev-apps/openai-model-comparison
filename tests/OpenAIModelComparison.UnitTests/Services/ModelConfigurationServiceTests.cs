using System.Net;
using OpenAIModelComparison.Models;
using OpenAIModelComparison.Services;
using OpenAIModelComparison.UnitTests.TestDoubles;

namespace OpenAIModelComparison.UnitTests.Services;

/// <summary>
/// Tests loading model configuration data.
/// </summary>
public class ModelConfigurationServiceTests
{
    /// <summary>
    /// Verifies that models and the configuration version are loaded from the expected endpoint.
    /// </summary>
    [Fact]
    public async Task LoadModelsAsync_LoadsModelsAndVersionFromExpectedEndpoint()
    {
        const string json = """
            {
              "version": "2026.11.18",
              "models": [
                {
                  "name": "gpt-6.2-sol",
                  "displayName": "GPT-6.2 Sol",
                  "isSelected": true,
                  "inputPricePerMillionTokens": 2,
                  "outputPricePerMillionTokens": 10
                }
              ]
            }
            """;
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson(json);
        ModelConfigurationService service = CreateService(handler);

        List<OpenAIModel> models = await service.LoadModelsAsync();

        OpenAIModel model = Assert.Single(models);
        Assert.Equal("gpt-6.2-sol", model.Name);
        Assert.True(model.IsSelected);
        Assert.Equal("2026.11.18", service.Version);
        Assert.Equal(
            "https://localhost/data/models.json",
            handler.LastRequest?.RequestUri?.ToString());
    }

    /// <summary>
    /// Verifies that an HTTP failure produces an empty model list.
    /// </summary>
    [Fact]
    public async Task LoadModelsAsync_WithHttpError_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = new(_ =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError));
        ModelConfigurationService service = CreateService(handler);

        List<OpenAIModel> models = await service.LoadModelsAsync();

        Assert.Empty(models);
        Assert.Null(service.Version);
    }

    /// <summary>
    /// Verifies that invalid JSON produces an empty model list.
    /// </summary>
    [Fact]
    public async Task LoadModelsAsync_WithInvalidJson_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson("{ invalid }");
        ModelConfigurationService service = CreateService(handler);

        List<OpenAIModel> models = await service.LoadModelsAsync();

        Assert.Empty(models);
        Assert.Null(service.Version);
    }

    /// <summary>
    /// Creates a model configuration service using the supplied handler.
    /// </summary>
    /// <param name="handler">The handler that provides HTTP responses.</param>
    /// <returns>The configured service.</returns>
    private static ModelConfigurationService CreateService(TestHttpMessageHandler handler) =>
        new(new HttpClient(handler), new TestNavigationManager());
}
