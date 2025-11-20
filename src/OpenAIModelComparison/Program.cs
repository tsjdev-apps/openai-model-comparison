using OpenAIModelComparison.Components;
using OpenAIModelComparison.Services;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRadzenComponents();
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Add HttpClient for loading configuration files
builder.Services.AddHttpClient<ModelConfigurationService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"] ?? "http://localhost:5000/");
});

// Add ModelConfigurationService
builder.Services.AddScoped<ModelConfigurationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
