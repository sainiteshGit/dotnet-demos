using RedisCacheDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
// Register ProductService
builder.Services.AddScoped<ProductService>();
// Configure Cache
var useRedis = false; // Change to true when Redis is accessible
var redisConnection = builder.Configuration["Redis:Connection"];

if (useRedis && !string.IsNullOrEmpty(redisConnection))
{
    try
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
            options.InstanceName = "RedisCacheDemo:";
        });
        Console.WriteLine("Redis cache configured successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not configure Redis: {ex.Message}");
        Console.WriteLine("Falling back to in-memory cache.");
        builder.Services.AddDistributedMemoryCache();
    }
}
else
{
    // Using in-memory distributed cache for demo
    Console.WriteLine("📦 Using in-memory distributed cache for demo.");
    Console.WriteLine("   (Set useRedis = true in Program.cs once Azure Redis firewall allows your IP)");
    builder.Services.AddDistributedMemoryCache();
}

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
