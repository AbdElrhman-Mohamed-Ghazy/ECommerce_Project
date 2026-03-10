# 🛒 E-Commerce API

Production-ready **ASP.NET Core 8 Web API** for an e-commerce platform built using **Clean Architecture**, **CQRS**, and modern backend best practices.  
The project focuses on **scalability, maintainability, and layered security**, making it suitable as a real-world backend foundation.

---

# 🚀 Key Highlights

- Clean Architecture (Domain / Application / Infrastructure / API)
- CQRS implementation using MediatR
- Repository + Unit of Work patterns
- JWT Authentication & Role-Based Authorization
- Policy-Based Authorization (OwnerOrAdmin)
- Secure product image storage
- Order workflow state machine
- Rate limiting & CORS protection
- Centralized error handling
- Validation pipeline with FluentValidation
- Audit logging for sensitive operations

---

# 🏗 Architecture

The system follows **Clean Architecture** to keep business logic independent from frameworks and infrastructure.

Project layers:

- **Domain**  
  Contains core entities, enums, and business rules.

- **Application**  
  Contains use cases, CQRS handlers, DTOs, interfaces, and validation.

- **Infrastructure**  
  Implements persistence, repositories, file storage, and external services.

- **API**  
  Exposes REST endpoints, middleware, authentication, and request pipelines.

---

# 🔄 CQRS Implementation

The project uses **CQRS (Command Query Responsibility Segregation)** with **MediatR**.

- **Commands**
  - Create / Update / Delete operations
  - Mutate application state

- **Queries**
  - Retrieve data
  - No side effects

This separation improves **maintainability, testability, and scalability**.

---

# 🛍 Core Features

## Products & Categories

- Full CRUD operations
- Category-product relationship
- Filtering & querying capabilities
- Product image upload support

---

## 🛒 Shopping Cart

- User-specific carts
- Add / Update / Remove items
- Persistent cart storage

---

## 📦 Order Management

Atomic order creation with stock validation.

Order lifecycle:

```
Pending → Paid → Shipped → Delivered
           ↘
          Cancelled
```

Prevents inconsistent states between **orders and inventory**.

---

# 🛡 Security Architecture

Security is implemented using a **defense-in-depth strategy**.

## Authentication

- JWT-based authentication
- Secure token validation
- Claims-based identity

## Authorization

- **Role-Based Authorization**
  - Admin-only endpoints for management

- **Policy-Based Authorization**
  - Fine-grained access rules

Example policy:

```
OwnerOrAdmin
```

Ensures users can only access their own resources.

## IDOR Protection

Ownership checks prevent **Insecure Direct Object Reference attacks**.

---

# ⚡ Infrastructure Hardening

Security protections implemented at the API level:

- Enforced **HTTPS**
- Strict **CORS configuration**
- **Rate Limiting** against brute-force or flooding
- **Input validation** using FluentValidation

---

# 📑 Observability

The system includes monitoring and security auditing features.

- Authentication failure logging
- Suspicious activity tracking
- Audit logs for privileged operations
  - Role changes
  - Deletions
  - Admin actions

Each log includes:

- Actor
- Action
- Timestamp

---

# 🔄 Transaction Management

A dedicated **Unit of Work** ensures safe database transactions.

Guarantees:

- **Atomicity** → all operations succeed or none are applied
- **Consistency** → prevents partial writes
- **Integrity** during complex operations (Orders + Stock updates)

---

# 🧰 Tech Stack

- ASP.NET Core 8
- Entity Framework Core 8
- SQL Server
- MediatR
- FluentValidation
- Scrutor
- JWT Authentication
- Swagger / OpenAPI

---

# 📄 API Documentation

Swagger UI is enabled for interactive API exploration.

After running the project:

```
https://localhost:{port}/swagger
```

---

# ⚙ Getting Started

### 1️⃣ Clone the repository

```
git clone https://github.com/yourusername/ecommerce-api.git
```

### 2️⃣ Navigate to the project

```
cd ecommerce-api
```

### 3️⃣ Update connection string

Configure **SQL Server** connection in:

```
appsettings.json
```

### 4️⃣ Run database migrations

```
dotnet ef database update
```

### 5️⃣ Run the API

```
dotnet run
```

---

# 🎯 Project Goals

This project demonstrates how to build a **secure, scalable backend API** using modern **.NET architecture patterns**.

Key focuses:

- Clean architecture design
- Secure API development
- Real-world backend patterns
- Maintainable and testable codebase


