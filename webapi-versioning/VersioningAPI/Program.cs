using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using VersioningAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add API versioning services
// This configuration supports ALL four versioning strategies:
// 1. URL Path: /api/v1/customers
// 2. Query String: /api/products?api-version=1.0
// 3. Header: X-API-Version: 1.0
// 4. Media Type: Accept: application/json; version=1.0
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    
    // Enable reading version from multiple sources
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),        // Reads from URL path: /api/v{version}
        new QueryStringApiVersionReader("api-version"),  // Reads from query: ?api-version=1.0
        new HeaderApiVersionReader("X-API-Version"),     // Reads from header: X-API-Version: 1.0
        new MediaTypeApiVersionReader("version")         // Reads from media type: Accept: application/json; version=1.0
    );
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configure Swagger to properly show all API versions
    var provider = builder.Services.BuildServiceProvider()
        .GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerDoc(
            description.GroupName,
            new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "API Versioning Demo",
                Version = description.ApiVersion.ToString(),
                Description = $"API Version {description.ApiVersion}" +
                    (description.IsDeprecated ? " (Deprecated)" : "")
            });
    }
    
    // Add operation filter to show version parameters in Swagger
    options.OperationFilter<ApiVersionOperationFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetService<IApiVersionDescriptionProvider>();
        if (provider != null)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        }
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
