using System.Net;
using OpenAIModelComparison.Services;
using OpenAIModelComparison.UnitTests.TestDoubles;

namespace OpenAIModelComparison.UnitTests.Services;

public class ModelConfigurationServiceTests
{
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

        var models = await service.LoadModelsAsync();

        var model = Assert.Single(models);
        Assert.Equal("gpt-6.2-sol", model.Name);
        Assert.True(model.IsSelected);
        Assert.Equal("2026.11.18", service.Version);
        Assert.Equal(
            "https://localhost/data/models.json",
            handler.LastRequest?.RequestUri?.ToString());
    }

    [Fact]
    public async Task LoadModelsAsync_WithHttpError_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = new(_ =>
            new HttpResponseMessage(HttpStatusCode.InternalServerError));
        ModelConfigurationService service = CreateService(handler);

        var models = await service.LoadModelsAsync();

        Assert.Empty(models);
        Assert.Null(service.Version);
    }

    [Fact]
    public async Task LoadModelsAsync_WithInvalidJson_ReturnsEmptyList()
    {
        TestHttpMessageHandler handler = TestHttpMessageHandler.ReturningJson("{ invalid }");
        ModelConfigurationService service = CreateService(handler);

        var models = await service.LoadModelsAsync();

        Assert.Empty(models);
        Assert.Null(service.Version);
    }

    private static ModelConfigurationService CreateService(TestHttpMessageHandler handler) =>
        new(new HttpClient(handler), new TestNavigationManager());
}
