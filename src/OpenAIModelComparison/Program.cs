using OpenAIModelComparison.Components;
using OpenAIModelComparison.Services;
using Radzen;

// Application entry point and service configuration
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add Radzen Blazor components for UI
builder.Services.AddRadzenComponents();

// Add Razor components with interactive server-side rendering
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Register HttpClient for ModelConfigurationService with base URL from configuration
builder.Services.AddHttpClient<IModelConfigurationService, ModelConfigurationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? "http://localhost:5000/");
});

// Register HttpClient for SamplePromptService with base URL from configuration
builder.Services.AddHttpClient<ISamplePromptService, SamplePromptService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? "http://localhost:5000/");
});

// Register application services
builder.Services.AddScoped<IModelConfigurationService, ModelConfigurationService>();
builder.Services.AddScoped<IOpenAIComparisonService, OpenAIComparisonService>();
builder.Services.AddScoped<ISamplePromptService, SamplePromptService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    // Use exception handler and HSTS in production
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// Enable HTTPS redirection
app.UseHttpsRedirection();

// Map static assets and Razor components with interactive server rendering
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Start the application
app.Run();
