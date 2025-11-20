using OpenAIModelComparison.Components;
using OpenAIModelComparison.Services;
using Radzen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRadzenComponents();
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IModelConfigurationService, ModelConfigurationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? "http://localhost:5000/");
});

builder.Services.AddScoped<IModelConfigurationService, ModelConfigurationService>();
builder.Services.AddScoped<IOpenAIComparisonService, OpenAIComparisonService>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
