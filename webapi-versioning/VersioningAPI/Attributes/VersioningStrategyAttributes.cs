namespace VersioningAPI.Attributes;

/// <summary>
/// Mark endpoints that use Query String versioning strategy
/// Shows only api-version query parameter in Swagger
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class QueryStringVersioningAttribute : Attribute { }

/// <summary>
/// Mark endpoints that use Header versioning strategy
/// Shows only X-API-Version header parameter in Swagger
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HeaderVersioningAttribute : Attribute { }

/// <summary>
/// Mark endpoints that use Media Type versioning strategy
/// Shows version in Accept header in Swagger
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class MediaTypeVersioningAttribute : Attribute { }
