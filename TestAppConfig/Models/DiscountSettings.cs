namespace TestAppConfig.Models;

public class DiscountSettings
{
    public List<string> AllowedUserIds { get; set; } = new();
    public List<string> AllowedProductIds { get; set; } = new();
}
