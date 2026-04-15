# 🛒 E-Commerce API

**Production-ready ASP.NET Core 8 Web API** for a full-featured e-commerce platform, built using **Clean Architecture**, **CQRS**, and modern backend best practices.  
Designed for **scalability, maintainability, and security**, making it a solid foundation for real-world backend applications.  

---

## 🚀 Key Highlights
- **Clean Architecture**: Domain / Application / Infrastructure / API layers  
- **CQRS with MediatR**: Separation of Commands & Queries for maintainability & testability  
- **Repository + Unit of Work patterns** for transactional safety  
- **ASP.NET Core Identity**: Full authentication & user management  
  - Email confirmation  
  - Password reset / forgot password  
  - Refresh token support for secure session management  
- **JWT Authentication & Role-Based Authorization**  
- **Rate Limiting & CORS Protection** to prevent abuse  
- Centralized **error handling & validation pipeline** (FluentValidation)  
- Audit logging for sensitive operations  

---

## 🏗 Architecture
The project follows **Clean Architecture** principles to keep business logic independent of frameworks and infrastructure.  

**Layers**:
- **Domain**: Core entities, enums, and business rules  
- **Application**: Use cases, CQRS handlers, DTOs, interfaces, validation  
- **Infrastructure**: Persistence, repositories, file storage, external services  
- **API**: REST endpoints, middleware, authentication, request pipelines  

---

## 🔄 CQRS Implementation
**CQRS (Command Query Responsibility Segregation)** with MediatR:

- **Commands**: Create / Update / Delete operations → mutate state  
- **Queries**: Retrieve data → no side effects  

Benefits:
- Improved **maintainability**  
- Easier **testing**  
- Better **scalability**  

---

## 🛍 Core Features

### Products & Categories
- Full CRUD operations  
- Category-product relationships  
- Filtering & querying capabilities  
- Product image upload support  

### Shopping Cart
- User-specific carts  
- Add / Update / Remove items  
- Persistent cart storage  

### Order Management
- Atomic order creation with stock validation  

**Order lifecycle**:
```
Pending → Paid → Shipped → Delivered
        ↘
      Cancelled
```

Prevents inconsistent states between orders and inventory.  

---

## 🛡 Security Features
Implemented using **defense-in-depth** strategy:

### Authentication
- JWT-based authentication  
- ASP.NET Identity integration  
- Refresh token support for secure sessions  
- Email confirmation & forgot/reset password  

### Authorization
- Role-Based Authorization (Admin vs User)  
- Protection via ownership checks  

### Infrastructure Hardening
- Enforced HTTPS  
- Strict CORS configuration  
- Rate Limiting to prevent brute-force/flood attacks  
- Input validation with FluentValidation  

---

## 📑 Observability & Auditing
- Authentication failure logs  
- Suspicious activity tracking  
- Audit logs for privileged operations:
  - Role changes  
  - Deletions  
  - Admin actions  
- Each log includes: **Actor, Action, Timestamp**  

---

## 🔄 Transaction Management
- **Unit of Work** ensures safe database transactions  

Guarantees:
- **Atomicity** → all operations succeed or none  
- **Consistency** → prevents partial writes  
- **Integrity** during complex operations (Orders + Stock updates)  

---

## 🧰 Tech Stack
| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web framework |
| Entity Framework Core 8 | ORM & migrations |
| SQL Server | Database |
| MediatR | CQRS pipeline |
| FluentValidation | Input validation |
| Scrutor | DI assembly scanning |
| ASP.NET Identity | Mange Users |
| Swagger / OpenAPI | API documentation |

---

## 📄 API Documentation
**Swagger UI** enabled for interactive API exploration:
```
https://localhost:{port}/swagger
```

---

## ⚙ Getting Started
```bash
# 1️⃣ Clone the repository
git clone https://github.com/AbdElrhman-Mohamed-Ghazy/ecommerce-api.git

# 2️⃣ Navigate to project
cd ecommerce-api

# 3️⃣ Configure connection string
# Edit appsettings.json with your SQL Server connection string

# 4️⃣ Run database migrations
dotnet ef database update

# 5️⃣ Run the API
dotnet run
```

---

## 🎯 Project Goals
This project demonstrates how to build a secure, scalable backend API using modern .NET patterns.

Key focuses:
- Clean architecture design  
- Secure API development with Identity & JWT  
- Real-world backend patterns  
- Maintainable, testable, production-ready codebase



