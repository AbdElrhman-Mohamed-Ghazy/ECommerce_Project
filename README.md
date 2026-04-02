# E-Commerce REST API

ASP.NET Core 8 Web API implementing a multi-domain e-commerce backend. Built with Clean Architecture, CQRS via MediatR, JWT authentication, and a role-based authorization model. All business logic is encapsulated in isolated MediatR handlers, keeping controllers thin and the domain free of framework dependencies.

---

## Table of Contents

1. [Architecture Deep Dive](#architecture-deep-dive)
2. [Database & Data Access](#database--data-access)
3. [API Layer](#api-layer)
4. [Core Features](#core-features)
5. [Cross-Cutting Concerns](#cross-cutting-concerns)
6. [Tech Stack](#tech-stack)
7. [Getting Started](#getting-started)
8. [CV Project Highlights](#cv-project-highlights)

---

## Architecture Deep Dive

The solution is split into four projects following Clean Architecture's dependency rule — outer layers depend on inner layers, never the reverse.

```
Domain          ← no external dependencies
   ↑
Application     ← depends on Domain
   ↑
Infrastructure  ← depends on Application (implements interfaces)
   ↑
E-CommerceAPI   ← depends on Application + Infrastructure (composition root)
```

### Domain (`Domain/`)

Pure C# class library with zero framework dependencies. Contains:

- **Entities**: `Product`, `Category`, `Order`, `OrderItem`, `Cart`, `CartItem`, `ProductImage`, `RefreshToken`, `ApplicationUser` (extends `IdentityUser`).
- **Enum**: `OrderStatus` (`Pending=0`, `Paid=1`, `Shipped=2`, `Delivered=3`, `Cancelled=4`).
- **Encapsulated behaviour on aggregates**: `Order.AddItem()`, `Order.ChangeStatus()`, `Cart.AddItem()`, `Cart.UpdateQuantity()`, `Cart.RemoveItem()`, `Cart.Clear()`, `CartItem.Increase()`, `CartItem.SetQuantity()`.

Domain entities enforce invariants internally (e.g. `CartItem.SetQuantity` validates quantity > 0), preventing callers from leaving aggregates in an invalid state.

### Application (`Application/`)

Orchestration layer. Contains use-case handlers, interfaces, DTOs, validators, AutoMapper profiles, and the MediatR pipeline behavior.

- **CQRS**: 28+ handler classes spread across `Entities/{Product,Category,Order,Cart,ProductImage}/Commands|Queries`. Commands mutate state; Queries only read. The split makes it straightforward to optimize read-side queries independently of write logic.
- **MediatR Pipeline Behavior** (`Behaviors/ValidationBehavior<TRequest,TResponse>`): Runs all FluentValidation validators registered for a request type before the handler executes. Throws `ValidationException` on failure, which is caught by the global exception handler.
- **Interfaces**: `IGenericRepository<T>`, `IProductRepository`, `IOrderRepository`, `ICartRepository`, `ICategoryRepository`, `IProductImageRepository`, `IUnitOfWorkRepository`, `IUserService` — the Application layer depends only on these abstractions.

### Infrastructure (`Infrastructure/`)

Implements all Application interfaces against SQL Server.

- `AppDbContext` : `IdentityDbContext<ApplicationUser>` — all EF Core configuration.
- Entity type configurations in `Infrastructure/Configuration/` (one class per entity, applied via `ApplyConfigurationsFromAssembly`).
- Repository implementations in `Infrastructure/Data/RepositoryImplementation/`.
- `UserService` in `Infrastructure/Services/` — wraps `UserManager<ApplicationUser>` / `SignInManager` for identity operations.

### API (`E-CommerceAPI/`)

Composition root and presentation layer.

- **Controllers** (`Controllers/`): Thin — each action method constructs a MediatR request object and dispatches it via `IMediator.Send()`. No business logic in controllers.
- **Global Exception Handler** (`Exceptions/GlobalExceptionHandler.cs`): `IExceptionHandler` implementation registered with `AddProblemDetails()`.
- **Program.cs**: All service registrations (DI, MediatR, FluentValidation, EF Core, Identity, JWT, Scrutor scan, Rate Limiting, Swagger).

---

## Database & Data Access

### EF Core Configuration

`AppDbContext` extends `IdentityDbContext<ApplicationUser>` and registers the following `DbSet<T>` properties:

| DbSet | Table |
|---|---|
| `Products` | `Products` |
| `Categories` | `Categories` |
| `Orders` | `Orders` |
| `OrderItems` | `OrderItems` |
| `Carts` | `Carts` |
| `CartItems` | `CartItems` |
| `ProductImages` | `ProductImages` |
| `RefreshTokens` | `RefreshTokens` |
| Identity tables | `security.*` schema |

Identity tables (`Users`, `Roles`, `UserRoles`, `UserClaims`, `UserLogins`, `RoleClaims`, `UserTokens`) are isolated in the `security` schema via `OnModelCreating` overrides.

Fluent API configurations in `Infrastructure/Configuration/`:

- `ProductConfiguration` — `Name` VARCHAR(200) NOT NULL; `Price` decimal(18,2); relationships to `Category`, `OrderItem`, `CartItem`, `ProductImage`.
- `OrderConfiguration` — `Status` stored as VARCHAR(50) string conversion; `TotalPrice` decimal(18,2); `ShippingAddress` VARCHAR(150).
- `CartItemConfiguration`, `OrderItemConfiguration` — composite foreign keys to parent aggregates and `Product`.
- `RefreshTokenConfiguration` — FK to `ApplicationUser`; stores token as a BCrypt hash (`RefreshTokenHash`).

### Repository Pattern

`IGenericRepository<T>` defines: `AddAsync`, `UpdateAsync`, `DeleteAsync`, `GetByIdAsync`, `GetAllAsync`, `FindAsync`, `SaveChangesAsync`.

Specialized interfaces extend the generic contract:

- **`IProductRepository`** adds: `GetProductsByName`, `GetProductsByCategoryName`, `IsExistAsync`, `GetProductPriceAsync`, `GetProductQuantityAsync`, `SetProductQuantityAsync`, `GetAllProductsByIds` (batch).
- **`IOrderRepository`** adds: `GetByUserIdAsync`, `GetByOrderIdAsync`, `GetFullOrderInfoByOrderIdAsync` (eager-loads `Items → Product`).
- **`ICartRepository`** adds: `GetByUserIdAsync` (eager-loads `Items`).
- **`IUnitOfWorkRepository`**: `SaveChangesAsync`, `BeginTransactionAsync` — used for multi-step transactional operations.

All repository classes are registered automatically by Scrutor scanning the `Infrastructure` assembly for types whose name ends with `"Repository"`, binding them to their implemented interfaces with Scoped lifetime.

### Migrations

Located in `Infrastructure/Migrations/`. Single migration (`20260326193955_Initial`) creates the full schema. Run with:

```bash
dotnet ef database update --project Infrastructure --startup-project E-CommerceAPI
```

---

## API Layer

### Controllers

| Controller | Route | Auth |
|---|---|---|
| `AccountController` | `api/Account` | Anonymous (most endpoints) |
| `Product` | `api/Product` | `[AllowAnonymous]` on GET; `[Authorize(Roles="Admin")]` on write |
| `Category` | `api/Category` | `[AllowAnonymous]` on GET; `[Authorize(Roles="Admin")]` on write |
| `Order` | `api/Order` | `[Authorize]` on all endpoints |
| `Cart` | `api/Cart` | No authorization attribute |
| `ProductImage` | `api/products/images` | `[Authorize(Roles="Admin")]` on all endpoints |

### Key Endpoints

**Account**
```
POST   api/Account/register
POST   api/Account/login              ← [RateLimiter: AuthLimiter — 5 req/min/IP]
POST   api/Account/refresh
POST   api/Account/logout
GET    api/Account/confirm-email?userId=&token=
POST   api/Account/forgot-password?email=
POST   api/Account/reset-password
```

**Products**
```
GET    api/Product/GetAllProducts
GET    api/Product/GetProductById/{productId}
GET    api/Product/GetProductByName?productName=
GET    api/Product/by-GetProductByCategoryName?categoryName=
POST   api/Product/AddProduct                          [Admin]
PUT    api/Product/UpdateProductbyId/{productId}       [Admin]
DELETE api/Product/DeleteProductbyId/{productId}       [Admin]
```

**Orders**
```
POST   api/Order/AddOrder?userId=&Address=             [Authorized]
GET    api/Order/GetOrderById/{orderId}                [Authorized]
GET    api/Order/GetOrdersByUserId/{userId}            [Authorized]
GET    api/Order/GetAllOrders                          [Authorized]
GET    api/Order/GetFullOrderInfoById/{orderId}        [Authorized]
PUT    api/Order/UpdateOrderStatus/{orderId}           [Authorized]
```

**Cart**
```
POST   api/Cart/CreateCart/{userId}
GET    api/Cart/GetCartByUserId/{userId}
POST   api/Cart/AddItemToCart/{userId}
PUT    api/Cart/UpdateItemQuantity/{userId}/{productId}
DELETE api/Cart/RemoveItemFromCart/{userId}/{productId}
DELETE api/Cart/ClearCart/{userId}
```

**Product Images**
```
POST   api/products/images/{productId}   [Admin] — multipart IFormFile
GET    api/products/images/{productId}   [Admin]
DELETE api/products/images/{imageId}     [Admin]
```

---

## Core Features

### Product Management

**Handlers**: `CreateProductCommandHandler`, `UpdateProductCommandHandler`, `DeleteProductCommandHandler`, `GetAllProductsListQueryHandler`, `GetProductByIdQueryHandler`, `GetProductsByNameQueryHandler`, `GetProductsByCategoryQueryHandler`.

`CreateProductCommandHandler` validates category existence via `ICategoryRepository.IsExistAsync` before persisting. `GetAllProductsListQueryHandler` uses AutoMapper to project `Product → ProductDto`. `GetProductsByCategoryQueryHandler` delegates filtering to `IProductRepository.GetProductsByCategoryName`, keeping the query optimised at the data layer.

### Order Creation (Transactional)

**Handler**: `CreateOrderCommandHandler`.

Wraps the entire operation in a database transaction via `IUnitOfWorkRepository.BeginTransactionAsync`:
1. Loads the user's cart via `ICartRepository.GetByUserIdAsync`.
2. For each `CartItem`, retrieves current stock with `IProductRepository.GetProductQuantityAsync` and validates availability.
3. Creates a new `Order` aggregate and calls `order.AddItem(productId, quantity, unitPrice)` for each item — updating `TotalPrice` atomically within the domain object.
4. Calls `IProductRepository.SetProductQuantityAsync` to decrement stock.
5. Persists the order and commits the transaction. On failure the transaction rolls back, preventing partial stock deductions.

### Order Status Management

**Handler**: `OrderStatusHandler`. Accepts an `OrderStatusCommand` with the target `OrderStatus` and calls `Order.ChangeStatus(status)` on the loaded aggregate before saving.

### Cart Management

**Handlers**: `CreateCartCommandHandler`, `AddItemToCartCommandHandler`, `UpdateCartItemQuantityCommandHandler`, `RemoveItemFromCartCommandHandler`, `ClearCartCommandHandler`, `GetCartInfoByUserIdHandler`.

`AddItemToCartCommandHandler` calls `Cart.AddItem(productId, price, quantity)` — the domain method either increments an existing `CartItem` quantity (via `CartItem.Increase`) or adds a new one, maintaining encapsulation of cart state transitions.

### Product Image Management

**Handlers**: `AddProductImagesHandler`, `DeleteProductImageHandler`, `GetProductImagesHandler`, `GetProductImageHandler`.

`AddProductImagesHandler` accepts `IFormFile` uploads and persists `ProductImage` entities with `ImageUrl`, `FilePath`, `FileName`, `FileExtension`, and `ContentType` stored in the database.

### Authentication & Token Management

**Service**: `UserService` in `Infrastructure/Services/`.

- **Registration**: Creates `ApplicationUser`, assigns role, generates email confirmation token via `UserManager.GenerateEmailConfirmationTokenAsync`.
- **Login**: Verifies credentials, generates a signed JWT (`JwtSecurityToken`) using the symmetric key from `JwtSettings`, and generates a refresh token stored as a BCrypt hash in `RefreshToken.RefreshTokenHash`.
- **Token Refresh**: `RefreshTokenAsync` validates the incoming token against the stored hash using BCrypt, issues a new JWT on success.
- **Logout**: Sets `RefreshToken.IsRevoked = true` in the database.
- **Password Reset**: Uses `UserManager.GeneratePasswordResetTokenAsync` / `ResetPasswordAsync`.

---

## Cross-Cutting Concerns

### Validation

FluentValidation validators (`CreateProductCommandValidator`, `UpdateProductCommandValidator`, `CreateCategoryCommandValidator`, `UpdateCategoryCommandValidator`, `CreateOrderCommandValidator`, `AddItemToCartCommandValidator`, `UpdateCartItemQuantityCommandValidator`, `ClearCartCommandValidator`, `RemoveItemFromCartCommandValidator`) are auto-registered via `AddValidatorsFromAssembly`.

`ValidationBehavior<TRequest, TResponse>` (MediatR `IPipelineBehavior`) runs before every handler. It aggregates all validation failures and throws `ValidationException`, short-circuiting handler execution. `CreateProductCommandValidator` includes an async rule that calls `ICategoryRepository.IsExistAsync` to validate referential integrity at the application layer before any database write.

### Exception Handling

`GlobalExceptionHandler` (`E-CommerceAPI/Exceptions/GlobalExceptionHandler.cs`) implements `IExceptionHandler` and is registered via `app.UseExceptionHandler()`. Mapping:

| Exception Type | HTTP Status |
|---|---|
| `FluentValidation.ValidationException` | 400 Bad Request |
| `NotFoundException` (Application layer) | 404 Not Found |
| `ValidationFaliedException` (Application layer) | 400 Bad Request |
| All others | 500 Internal Server Error |

Responses use `ProblemDetails` format (`AddProblemDetails()` registered in DI).

### Authentication & Authorization

JWT Bearer authentication is the default scheme. `TokenValidationParameters` enforce: Issuer, Audience, token lifetime, and HMAC-SHA symmetric key signature.

Authorization:
- `[Authorize(Roles = "Admin")]` on all Product write endpoints (`AddProduct`, `UpdateProductbyId`, `DeleteProductbyId`), all Category write endpoints, and all ProductImage endpoints.
- `[Authorize]` on all Order endpoints.
- `options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"))` registered but available for future use.
- `[AllowAnonymous]` on all Product and Category GET endpoints.

### Rate Limiting

`AuthLimiter` policy uses a fixed-window limiter: 5 requests per 60-second window, partitioned by client IP (`RemoteIpAddress`). Applied only to `POST api/Account/login` via `[EnableRateLimiting("AuthLimiter")]`. Rejection returns HTTP 429.

---

## Tech Stack

| Library / Framework | Version | Purpose |
|---|---|---|
| ASP.NET Core | 8.0 | Web framework, middleware pipeline, DI container |
| Entity Framework Core | 8.0.24 | ORM, migrations, query generation |
| EF Core SQL Server provider | 8.0.24 | SQL Server data access |
| EF Core SQLite provider | 8.0.24 | Alternative/test database provider |
| ASP.NET Core Identity (EF) | 8.0.24 | User/role management, password hashing, token providers |
| MediatR | 14.0.0 | CQRS dispatcher, pipeline behaviors |
| AutoMapper | 16.1.1 | Object-to-object mapping (entity → DTO) |
| FluentValidation.DependencyInjectionExtensions | 12.1.1 | Input validation, auto-registration |
| BCrypt.Net-Next | 4.1.0 | Refresh token hashing |
| Microsoft.AspNetCore.Authentication.JwtBearer | 8.0.24 | JWT validation middleware |
| Scrutor.AspNetCore | 3.3.0 | Convention-based DI registration (repository scan) |
| Swashbuckle.AspNetCore | 6.6.2 | Swagger / OpenAPI documentation |

---

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or update the connection string for SQLite)

### Setup

```bash
# 1. Clone the repository
git clone https://github.com/AbdElrhman-Mohamed-Ghazy/ECommerce_Project.git
cd ECommerce_Project

# 2. Configure connection string in E-CommerceAPI/appsettings.json
#    "ConnectionStrings": { "constr": "Server=.;Database=E-CommerceAPI;Integrated Security=True;TrustServerCertificate=True" }

# 3. Apply migrations
dotnet ef database update --project Infrastructure --startup-project E-CommerceAPI

# 4. Run the API
dotnet run --project E-CommerceAPI
```

Swagger UI is available at `https://localhost:{port}/swagger` in the Development environment. Use the Bearer token scheme in Swagger to test authenticated endpoints.

---

## CV Project Highlights

- **Designed and implemented a multi-domain e-commerce REST API** in ASP.NET Core 8 using Clean Architecture (Domain / Application / Infrastructure / API), enforcing strict dependency inversion — the Domain and Application layers have zero framework or infrastructure dependencies.

- **Implemented CQRS using MediatR 14** with 28+ segregated command and query handlers across Product, Order, Cart, Category, and ProductImage domains; a `ValidationBehavior<TRequest, TResponse>` pipeline behavior auto-runs FluentValidation validators (including async cross-entity checks) before every handler, eliminating validation boilerplate from business logic.

- **Built a transactional order-creation flow** (`CreateOrderCommandHandler`) using `IUnitOfWorkRepository.BeginTransactionAsync`: atomically converts a user's cart into an `Order` aggregate, decrements product stock via `IProductRepository.SetProductQuantityAsync`, and rolls back on any failure — preventing partial inventory mutations.

- **Secured the API with JWT Bearer authentication and ASP.NET Core Identity**: refresh tokens stored as BCrypt hashes, role-based access control (`[Authorize(Roles="Admin")]`) on all write endpoints, a fixed-window rate limiter (5 req/min/IP) on the login endpoint, and a centralized `GlobalExceptionHandler` mapping domain exceptions to RFC 7807 `ProblemDetails` responses.

