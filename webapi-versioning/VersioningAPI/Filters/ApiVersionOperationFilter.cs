using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using VersioningAPI.Attributes;

namespace VersioningAPI.Filters;

/// <summary>
/// Swagger operation filter to add version parameters for non-path-based versioning
/// Only adds the specific parameter(s) for each strategy based on attributes
/// </summary>
public class ApiVersionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Skip if this is a path-based versioning endpoint (has {version:apiVersion} in route)
        if (context.ApiDescription.RelativePath.Contains("{version:apiVersion}"))
            return;

        var apiVersion = context.ApiDescription.GroupName;
        
        if (operation.Parameters == null)
            operation.Parameters = new List<OpenApiParameter>();

        // Get the method's attributes to determine which strategy it uses
        var methodInfo = context.ApiDescription.ActionDescriptor?.EndpointMetadata
            ?.Select(m => m.GetType())
            .FirstOrDefault() ?? typeof(object);

        var attributes = context.ApiDescription.ActionDescriptor?.EndpointMetadata ?? Array.Empty<object>();

        // Add only the parameter for the specific strategy marked by attribute
        if (attributes.OfType<QueryStringVersioningAttribute>().Any())
        {
            // Only add if not already present
            if (!operation.Parameters.Any(p => p.Name == "api-version" && p.In == ParameterLocation.Query))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "api-version",
                    In = ParameterLocation.Query,
                    Description = "API version (Query String Strategy)",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(apiVersion) }
                });
            }
        }
        else if (attributes.OfType<HeaderVersioningAttribute>().Any())
        {
            // Only add if not already present
            if (!operation.Parameters.Any(p => p.Name == "X-API-Version" && p.In == ParameterLocation.Header))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-API-Version",
                    In = ParameterLocation.Header,
                    Description = "API version (Header Strategy)",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string", Default = new Microsoft.OpenApi.Any.OpenApiString(apiVersion) }
                });
            }
        }
        else if (attributes.OfType<MediaTypeVersioningAttribute>().Any())
        {
            // Media Type versioning: version is in Accept header
            // Only add if not already present
            if (!operation.Parameters.Any(p => p.Name == "Accept" && p.In == ParameterLocation.Header))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Accept",
                    In = ParameterLocation.Header,
                    Description = "Media type with version (e.g., application/json; version=1.0)",
                    Required = false,
                    Schema = new OpenApiSchema 
                    { 
                        Type = "string", 
                        Default = new Microsoft.OpenApi.Any.OpenApiString("application/json; version=" + apiVersion)
                    }
                });
            }
        }
    }
}
