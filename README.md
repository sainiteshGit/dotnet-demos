# .NET Demos

A collection of .NET 8 sample projects demonstrating various patterns and Azure integrations.

## 🚀 Projects

### 1. RedisCacheDemo
**Distributed Caching with Azure Managed Redis**

Demonstrates the cache-aside pattern using `IDistributedCache` abstraction with Azure Managed Redis.

**Features:**
- Cache-aside pattern implementation
- Redis and in-memory cache toggle for local development
- Cache HIT/MISS logging
- Configurable expiration policies (absolute & sliding)

**Run:**
```bash
cd RedisCacheDemo
dotnet run
```

**Endpoints:**
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products (cached) |
| GET | `/api/products/{id}` | Get product by ID (cached) |
| DELETE | `/api/products/cache/{id}` | Invalidate cache for product |
| GET | `/api/products/debug` | View cache statistics |

---

### 2. TestAppConfig
**Azure App Configuration Demo**

Demonstrates integration with Azure App Configuration for centralized configuration management.

**Features:**
- Azure App Configuration integration
- Settings binding with strongly-typed models
- Dynamic configuration refresh

**Run:**
```bash
cd TestAppConfig
dotnet run
```

---

### 3. VersioningAPI
**API Versioning Strategies**

Demonstrates different API versioning approaches in ASP.NET Core.

**Features:**
- URL path versioning (`/api/v1/...`)
- Query string versioning (`?api-version=1.0`)
- Header-based versioning (`X-Api-Version`)
- Swagger/OpenAPI integration with version selection

**Run:**
```bash
cd webapi-versioning/VersioningAPI
dotnet run
```

---

## 🛠️ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- (Optional) Azure subscription for cloud services

## 📦 Getting Started

```bash
# Clone the repository
git clone https://github.com/sainiteshGit/dotnet-demos.git
cd dotnet-demos

# Restore and build all projects
dotnet restore
dotnet build

# Run a specific project
cd RedisCacheDemo
dotnet run
```

## 🔧 Configuration

Each project has its own `appsettings.json` and `appsettings.Development.json` for environment-specific settings.

**For Azure services:**
1. Create the required Azure resources
2. Update connection strings in `appsettings.json`
3. For local development, use User Secrets or environment variables

## 📝 License

This project is for demonstration purposes.

## 👤 Author

**Sai Nitesh** - [GitHub](https://github.com/sainiteshGit)
