using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisCacheDemo.Services;

namespace RedisCacheDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly IDistributedCache _cache;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        ProductService productService,
        IDistributedCache cache,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _cache = cache;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(string id)
    {
        var product = await _productService.GetProductAsync(id);

        if (product == null)
            return NotFound(new { message = $"Product {id} not found" });

        return Ok(product);
    }

    [HttpDelete("cache/{id}")]
    public async Task<IActionResult> InvalidateCache(string id)
    {
        await _productService.InvalidateCacheAsync(id);
        return Ok(new { message = $"Cache invalidated for product {id}" });
    }

    [HttpDelete("cache")]
    public async Task<IActionResult> InvalidateAllCache()
    {
        // Invalidate known cache keys
        await _cache.RemoveAsync("products:all");
        for (int i = 1; i <= 5; i++)
        {
            await _cache.RemoveAsync($"product:{i}");
        }

        return Ok(new { message = "All product cache invalidated" });
    }

    [HttpGet("debug")]
    public IActionResult Debug()
    {
        return Ok(new
        {
            cacheType = _cache.GetType().Name,
            timestamp = DateTime.UtcNow,
            message = "If cacheType is 'RedisCache', you're connected to Azure Redis!"
        });
    }
}
