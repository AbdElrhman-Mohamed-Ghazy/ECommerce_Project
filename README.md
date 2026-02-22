# 🛒 E-Commerce API

ASP.NET Core 8.0 backend for an e-commerce system built with a **clear separation of concerns**
and a **code-first architecture**.  
The project emphasizes **scalability**, **maintainability**, and **well-structured layers**
using proven architectural and design patterns.

---

## 🏗 Architecture

- Clean Architecture with isolated **API**, **Application**, **Domain**, and **Infrastructure** layers
- CQRS implementation using MediatR for strict command/query separation
- Repository and Unit of Work patterns for data persistence and transactional consistency
- Automatic dependency registration using Scrutor
- Request validation via FluentValidation integrated through MediatR pipeline behaviors
- Centralized exception handling with consistent `ProblemDetails` responses

---

## ✨ Core Features (Code-Based)

### Products
- Create, update, delete, and retrieve products
- Search products by name
- Filter products by category

### Categories
- Full CRUD operations
- Enforced relational integrity with products

### Shopping Cart
- User-scoped shopping cart
- Add, update, remove, and clear cart items

### Orders
- Order lifecycle managed through a finite state workflow:
  **Pending → Paid → Shipped → Delivered / Cancelled**
- Stock validation and transactional order creation

### Product Images
- Upload, retrieve, and delete product images
- File system–based storage implementation

---

## 🧰 Technologies

- ASP.NET Core 8.0
- Entity Framework Core 8.0.24
- SQL Server
- MediatR
- FluentValidation
- Scrutor
- Swagger / Swashbuckle

---

## 🔄 Transaction Management

- Explicit **Unit of Work** implementation
- Ensures atomic operations across multiple repositories
- Prevents partial data persistence during order processing

---
🛠️ Roadmap (Upcoming Features)
We are constantly improving the system. The following features are planned for future releases:

[ ] Authentication & Authorization: Implementing ASP.NET Core Identity with JWT (JSON Web Tokens) for secure user access.

[ ] Role-Based Access Control (RBAC): Defining permissions for Customers and Administrators.

[ ] Caching Layer: Integrating Redis to improve performance for product catalogs.

[ ] Advanced Searching: Integrating Elasticsearch for faster and more relevant product searches.
