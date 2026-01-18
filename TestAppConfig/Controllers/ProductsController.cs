using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement;
using TestAppConfig.Models;

namespace TestAppConfig.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IFeatureManager _featureManager;
    private readonly IOptionsSnapshot<DiscountSettings> _optionsSnapshot;
    private readonly ILogger<ProductsController> _logger;
    private readonly IConfiguration _configuration;

    public ProductsController(
        IFeatureManager featureManager,
        IOptionsSnapshot<DiscountSettings> optionsSnapshot,
        ILogger<ProductsController> logger,
        IConfiguration configuration)
    {
        _featureManager = featureManager;
        _optionsSnapshot = optionsSnapshot;
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet("debug-config")]
    public async Task<IActionResult> GetDebugConfig()
    {
        // Get raw values from IConfiguration
        var configUsers = _configuration["DiscountSettings:AllowedUserIds"];
        var configProducts = _configuration["DiscountSettings:AllowedProductIds"];
        
        // Check feature flag status
        var featureEnabled = await _featureManager.IsEnabledAsync("ApplyDiscount");

        _logger.LogInformation("DEBUG - IConfiguration Values - Users: {Users}, Products: {Products}", configUsers, configProducts);
        _logger.LogInformation("DEBUG - IOptionsSnapshot Values - Users: [{Users}], Products: [{Products}]",
            string.Join(", ", _optionsSnapshot.Value.AllowedUserIds),
            string.Join(", ", _optionsSnapshot.Value.AllowedProductIds));
        _logger.LogInformation("DEBUG - Feature Flag - ApplyDiscount: {FeatureEnabled}", featureEnabled);

        return Ok(new
        {
            featureFlag = new
            {
                applyDiscountEnabled = featureEnabled
            },
            ioptionsSnapshotValues = new
            {
                allowedUserIds = _optionsSnapshot.Value.AllowedUserIds,
                allowedProductIds = _optionsSnapshot.Value.AllowedProductIds
            }
        });
    }
}
