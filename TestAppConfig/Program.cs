using Azure.Identity;
using Microsoft.Azure.AppConfiguration.AspNetCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.FeatureManagement;
using TestAppConfig.Models;

var builder = WebApplication.CreateBuilder(args);

// Connect to Azure App Configuration (optional - only if valid connection string provided)
var appConfigConnection = builder.Configuration["AppConfig:Connection"];

if (!string.IsNullOrEmpty(appConfigConnection) && appConfigConnection.StartsWith("Endpoint="))
{
    try
    {
        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            options.Connect(appConfigConnection)
                   .Select(KeyFilter.Any, LabelFilter.Null)
                   .ConfigureRefresh(refresh =>
                   {
                       refresh.RegisterAll()
                              .SetRefreshInterval(TimeSpan.FromSeconds(5));
                   });
        });
        
        // Add Azure App Configuration service for runtime refresh
        builder.Services.AddAzureAppConfiguration();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not connect to Azure App Configuration: {ex.Message}");
        Console.WriteLine("Using local configuration fallback only.");
    }
}

// Add Feature Management
builder.Services.AddFeatureManagement();

// Register configuration settings with hot reload support
// Standard pattern for IOptionsSnapshot
builder.Services.Configure<DiscountSettings>(
builder.Configuration.GetSection("DiscountSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

// Enable Azure App Configuration middleware for dynamic refresh
// This must be called early in the pipeline to enable automatic configuration updates
if (!string.IsNullOrEmpty(appConfigConnection) && appConfigConnection.StartsWith("Endpoint="))
{
    app.UseAzureAppConfiguration();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
