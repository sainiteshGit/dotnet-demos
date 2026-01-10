namespace VersioningAPI.Models;

public class CustomerV3
{
    public string CustomerId { get; set; } = string.Empty;  // Changed from int Id to string CustomerId
    public string FullName { get; set; } = string.Empty;     // Renamed from Name
    public List<string> EmailAddresses { get; set; } = new(); // Changed from single Email to list
    public string PrimaryPhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastModifiedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public CustomerAddressV3 Address { get; set; } = new();
}

public class CustomerAddressV3
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}
