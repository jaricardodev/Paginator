# Paginator

![Paginator Logo](paginator.png)

A set of .NET libraries that provide simple, reusable pagination support for ASP.NET Core applications backed by Entity Framework Core. The project is split into three focused NuGet packages so you can take only what you need.

## Packages

| Package | Description |
|---------|-------------|
| [`Jaricardodev.Paginator.Model`](#jaricardodevpaginatormodel) | Core pagination model and interface |
| [`Jaricardodev.Paginator.Persistence`](#jaricardodevpaginatorpersistence) | Entity Framework Core `IQueryable` extension for paginated queries |
| [`Jaricardodev.Paginator.ServiceHost`](#jaricardodevpaginatorservicehost) | ASP.NET Core result filter that automatically appends an `X-Pagination` response header |

---

## How the packages work together

```
Client  ──GET /products?page=2&pageSize=10──►  ASP.NET Core API
                                                    │
                                         ┌──────────▼──────────────┐
                                         │  Controller / Service   │
                                         │                         │
                                         │  dbContext.Products     │
                                         │    .ToPaginatedListAsync│  ◄── Persistence
                                         │      (page, pageSize)   │
                                         └──────────┬──────────────┘
                                                    │  PaginatedList<Product>
                                         ┌──────────▼──────────────┐
                                         │ AddPaginationHeader     │
                                         │ ResultFilter            │  ◄── ServiceHost
                                         │ (adds X-Pagination hdr) │
                                         └──────────┬──────────────┘
                                                    │
Client  ◄── 200 OK  ────────────────────────────────┘
        Body:    [ ...products for current page... ]
        Headers: X-Pagination: {"totalItemsCount":100,"totalPageCount":10}
```

---

## Jaricardodev.Paginator.Model

### Installation

```bash
dotnet add package Jaricardodev.Paginator.Model
```

or via the NuGet Package Manager Console:

```powershell
Install-Package Jaricardodev.Paginator.Model
```

### What it contains

| Type | Description |
|------|-------------|
| `IPaginatedList` | Interface that exposes `TotalItemsCount` and `TotalPageCount` |
| `PaginatedList<T>` | Generic list that implements both `IList<T>` and `IPaginatedList` |

### Usage

```csharp
using Jaricardodev.Paginator.Model.Capabilities;

// Create a paginated list manually
var page = new PaginatedList<string>();
page.Add("item1");
page.Add("item2");
page.TotalItemsCount = 50;
page.TotalPageCount  = 5;

// Implicit conversion from List<T>
List<string> source = new List<string> { "a", "b", "c" };
PaginatedList<string> paginated = source;   // implicit cast
paginated.TotalItemsCount = 3;
paginated.TotalPageCount  = 1;
```

### API reference

#### `IPaginatedList`

| Member | Type | Description |
|--------|------|-------------|
| `TotalItemsCount` | `int` | Total number of items across all pages |
| `TotalPageCount` | `int` | Total number of pages |

#### `PaginatedList<T>`

Implements `IList<T>` and `IPaginatedList`.  All standard `IList<T>` members are available (`Add`, `Remove`, `Contains`, `IndexOf`, `Insert`, `RemoveAt`, `Clear`, indexer, etc.).  In addition:

| Member | Description |
|--------|-------------|
| `TotalItemsCount` | Total items in the full data set |
| `TotalPageCount` | Total pages in the full data set |
| `implicit operator PaginatedList<T>(List<T>)` | Converts a `List<T>` to `PaginatedList<T>` |

---

## Jaricardodev.Paginator.Persistence

### Installation

```bash
dotnet add package Jaricardodev.Paginator.Persistence
```

or via the NuGet Package Manager Console:

```powershell
Install-Package Jaricardodev.Paginator.Persistence
```

> **Dependency:** Requires `Jaricardodev.Paginator.Model` and `Microsoft.EntityFrameworkCore` (≥ 5.0).

### What it contains

An extension method on `IQueryable<T>` that executes a paginated database query and returns a fully populated `PaginatedList<T>`.

### Usage

```csharp
using Jaricardodev.Paginator.Model.Capabilities;
using Jaricardodev.Paginator.Persistence.Extensions;

public class ProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db) => _db = db;

    public async Task<PaginatedList<Product>> GetPageAsync(int page, int pageSize)
    {
        return await _db.Products
            .AsNoTracking()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToPaginatedListAsync(page, pageSize);
    }
}
```

The extension method:
1. Executes a `COUNT` query to obtain `TotalItemsCount`.
2. Applies `Skip` / `Take` for the requested page.
3. Executes the data query asynchronously.
4. Calculates `TotalPageCount = (int)Math.Ceiling((double)totalCount / itemsPerPage)`.
5. Returns a `PaginatedList<T>` with all fields populated.

### API reference

#### `IQueryableExtensions`

```csharp
public static Task<PaginatedList<T>> ToPaginatedListAsync<T>(
    this IQueryable<T> source,
    int page,
    int itemsPerPage)
```

| Parameter | Description |
|-----------|-------------|
| `source` | The EF Core `IQueryable<T>` to paginate |
| `page` | 1-based page number |
| `itemsPerPage` | Number of items per page |

---

## Jaricardodev.Paginator.ServiceHost

### Installation

```bash
dotnet add package Jaricardodev.Paginator.ServiceHost
```

or via the NuGet Package Manager Console:

```powershell
Install-Package Jaricardodev.Paginator.ServiceHost
```

> **Dependency:** Requires `Jaricardodev.Paginator.Model` and the ASP.NET Core shared framework (`Microsoft.AspNetCore.App` framework reference), plus `Newtonsoft.Json` (≥ 13.0).

### What it contains

An ASP.NET Core `IAsyncResultFilter` that automatically adds an `X-Pagination` header to any response whose body implements `IPaginatedList`.

### Setup

Register the filter globally in `Program.cs` (or `Startup.cs`):

```csharp
using Jaricardodev.Paginator.ServiceHost.Filters;

// Program.cs (.NET 6+ minimal hosting)
builder.Services.AddControllers(options =>
{
    options.Filters.Add<AddPaginationHeaderResultFilter>();
});
```

For .NET 5 `Startup.cs`:

```csharp
using Jaricardodev.Paginator.ServiceHost.Filters;

public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers(options =>
    {
        options.Filters.Add(typeof(AddPaginationHeaderResultFilter));
    });
}
```

### Usage

No changes are needed in your controllers.  When the action result contains a `PaginatedList<T>`, the filter automatically adds the header:

```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<PaginatedList<ProductDto>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _service.GetPageAsync(page, pageSize);
        return Ok(result);   // X-Pagination header is added automatically
    }
}
```

**HTTP response example:**

```
HTTP/1.1 200 OK
Content-Type: application/json
X-Pagination: {"totalItemsCount":100,"totalPageCount":10}

[...items for the requested page...]
```

### API reference

#### `AddPaginationHeaderResultFilter`

Implements `IAsyncResultFilter`.  When the `ObjectResult` value implements `IPaginatedList`, the filter serialises `TotalItemsCount` and `TotalPageCount` as JSON and adds the result as the `X-Pagination` response header.

---

## License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).
