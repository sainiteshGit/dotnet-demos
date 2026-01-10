using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using VersioningAPI.Attributes;
using VersioningAPI.Models;

namespace VersioningAPI.Controllers;

/// <summary>
/// Unified API Versioning Demo - All Four Strategies in One Controller
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[ApiVersion("3.0")]
public class ApiDemoController : ControllerBase
{
    #region Sample Data

    private static readonly List<CustomerV1> CustomersV1 = new()
    {
        new CustomerV1 { Id = 1, Name = "John Doe", Email = "john@example.com" },
        new CustomerV1 { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
    };

    private static readonly List<CustomerV2> CustomersV2 = new()
    {
        new CustomerV2 { Id = 1, Name = "John Doe", Email = "john@example.com", PhoneNumber = "555-0001", CreatedAt = DateTime.Now },
        new CustomerV2 { Id = 2, Name = "Jane Smith", Email = "jane@example.com", PhoneNumber = "555-0002", CreatedAt = DateTime.Now }
    };

    private static readonly List<CustomerV3> CustomersV3 = new()
    {
        new CustomerV3 { CustomerId = "CUST-001", FullName = "John Doe", EmailAddresses = new() { "john@example.com" }, PrimaryPhoneNumber = "555-0001", CreatedAt = DateTime.Now, LastModifiedAt = DateTime.Now },
        new CustomerV3 { CustomerId = "CUST-002", FullName = "Jane Smith", EmailAddresses = new() { "jane@example.com" }, PrimaryPhoneNumber = "555-0002", CreatedAt = DateTime.Now, LastModifiedAt = DateTime.Now }
    };

    private static readonly List<dynamic> ProductsV1 = new()
    {
        new { id = 1, name = "Widget A", price = 29.99 },
        new { id = 2, name = "Widget B", price = 39.99 }
    };

    private static readonly List<dynamic> ProductsV2 = new()
    {
        new { id = 1, name = "Widget A", price = 29.99, stock = 100, discount = 0.1 },
        new { id = 2, name = "Widget B", price = 39.99, stock = 50, discount = 0.05 }
    };

    private static readonly List<dynamic> OrdersV1 = new()
    {
        new { id = 1001, customerId = 1, amount = 99.99, status = "Completed" },
        new { id = 1002, customerId = 2, amount = 149.99, status = "Pending" }
    };

    private static readonly List<dynamic> OrdersV2 = new()
    {
        new { id = 1001, customerId = 1, amount = 99.99, status = "Completed", currency = "USD", trackingNumber = "TRK-123456" },
        new { id = 1002, customerId = 2, amount = 149.99, status = "Pending", currency = "USD", trackingNumber = "TRK-789012" }
    };

    private static readonly List<dynamic> InvoicesV1 = new()
    {
        new { invoiceNumber = "INV-001", customerId = 1, amount = 299.99, isPaid = false },
        new { invoiceNumber = "INV-002", customerId = 2, amount = 450.50, isPaid = true }
    };

    private static readonly List<dynamic> InvoicesV2 = new()
    {
        new { invoiceNumber = "INV-001", customerId = 1, subtotal = 250.00, tax = 25.00, discount = 25.01, total = 299.99, isPaid = false },
        new { invoiceNumber = "INV-002", customerId = 2, subtotal = 400.00, tax = 40.00, discount = 0.00, total = 450.50, isPaid = true }
    };

    #endregion

    #region Strategy 1: URL Path Versioning
    // How it works: Version is in the URL path - /api/v{version}/customers
    // Client sends: GET /api/v1/customers  OR  GET /api/v2/customers
    // The {version:apiVersion} route constraint extracts version from URL

    [HttpGet("api/v{version:apiVersion}/customers")]
    [MapToApiVersion("1.0")]
    public ActionResult GetCustomersV1() => Ok(CustomersV1);

    [HttpGet("api/v{version:apiVersion}/customers")]
    [MapToApiVersion("2.0")]
    public ActionResult GetCustomersV2() => Ok(CustomersV2);

    [HttpGet("api/v{version:apiVersion}/customers")]
    [MapToApiVersion("3.0")]
    public ActionResult GetCustomersV3() => Ok(CustomersV3);
    #endregion

    #region Strategy 2: Query String Versioning
    // How it works: Version is in query string - ?api-version=1.0
    // Client sends: GET /api/products?api-version=1.0  OR  GET /api/products?api-version=2.0
    // The QueryStringApiVersionReader reads "api-version" parameter

    [HttpGet("api/products")]
    [MapToApiVersion("1.0")]
    [QueryStringVersioning]
    public ActionResult GetProductsV1() => Ok(ProductsV1);

    [HttpGet("api/products")]
    [MapToApiVersion("2.0")]
    [QueryStringVersioning]
    public ActionResult GetProductsV2() => Ok(ProductsV2);
    #endregion

    #region Strategy 3: Header-Based Versioning
    // How it works: Version is in HTTP header - X-API-Version: 1.0
    // Client sends: GET /api/orders with Header: X-API-Version: 1.0
    // Client sends: GET /api/orders with Header: X-API-Version: 2.0
    // The HeaderApiVersionReader reads "X-API-Version" header

    [HttpGet("api/orders")]
    [MapToApiVersion("1.0")]
    [HeaderVersioning]
    public ActionResult GetOrdersV1() => Ok(OrdersV1);

    [HttpGet("api/orders")]
    [MapToApiVersion("2.0")]
    [HeaderVersioning]
    public ActionResult GetOrdersV2() => Ok(OrdersV2);
    #endregion

    #region Strategy 4: Media Type Versioning
    // How it works: Version is in Accept header - Accept: application/json; version=1.0
    // Client sends: GET /api/invoices with Header: Accept: application/json; version=1.0
    // Client sends: GET /api/invoices with Header: Accept: application/json; version=2.0
    // The MediaTypeApiVersionReader reads "version" parameter from Accept header

    [HttpGet("api/invoices")]
    [MapToApiVersion("1.0")]
    [MediaTypeVersioning]
    public ActionResult GetInvoicesV1() => Ok(InvoicesV1);

    [HttpGet("api/invoices")]
    [MapToApiVersion("2.0")]
    [MediaTypeVersioning]
    public ActionResult GetInvoicesV2() => Ok(InvoicesV2);
    #endregion
}
