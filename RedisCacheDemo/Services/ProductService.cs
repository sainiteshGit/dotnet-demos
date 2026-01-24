using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RedisCacheDemo.Models;

namespace RedisCacheDemo.Services;

public class ProductService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProductService> _logger;

    // Simulated database
    private static readonly Dictionary<string, Product> _products = new()
    {
        ["1"] = new() { Id = "1", Name = "Laptop", Category = "Electronics", Price = 999.99m },
        ["2"] = new() { Id = "2", Name = "Mouse", Category = "Electronics", Price = 29.99m },
        ["3"] = new() { Id = "3", Name = "Keyboard", Category = "Electronics", Price = 79.99m },
        ["4"] = new() { Id = "4", Name = "Monitor", Category = "Electronics", Price = 349.99m },
        ["5"] = new() { Id = "5", Name = "Headphones", Category = "Electronics", Price = 149.99m }
    };

    public ProductService(IDistributedCache cache, ILogger<ProductService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<Product?> GetProductAsync(string productId)
    {
        var cacheKey = $"product:{productId}";

        // Step 1: Try cache first
        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache HIT for product {ProductId}", productId);
            return JsonSerializer.Deserialize<Product>(cached);
        }

        _logger.LogInformation("Cache MISS for product {ProductId}", productId);

        // Step 2: Get from "database"
        var product = await GetFromDatabaseAsync(productId);

        // Step 3: Store in cache
        if (product != null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(2)
            };

            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(product), options);
            _logger.LogInformation("Cached product {ProductId}", productId);
        }

        return product;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var cacheKey = "products:all";

        var cached = await _cache.GetStringAsync(cacheKey);
        if (cached != null)
        {
            _logger.LogInformation("Cache HIT for all products");
            return JsonSerializer.Deserialize<List<Product>>(cached) ?? new List<Product>();
        }

        _logger.LogInformation("Cache MISS for all products");

        var products = _products.Values.ToList();

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(products), options);

        return products;
    }

    public async Task InvalidateCacheAsync(string productId)
    {
        await _cache.RemoveAsync($"product:{productId}");
        await _cache.RemoveAsync("products:all");
        _logger.LogInformation("Cache invalidated for product {ProductId}", productId);
    }

    private Task<Product?> GetFromDatabaseAsync(string productId)
    {
        // Simulate database delay
        Task.Delay(100).Wait();
        return Task.FromResult(_products.GetValueOrDefault(productId));
    }
}
