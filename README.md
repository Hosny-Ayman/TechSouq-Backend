<<<<<<< HEAD
# TechSouq Backend API

Backend API for the TechSouq e-commerce application, built with ASP.NET Core 8 and SQL Server.

The API handles authentication, products, categories, brands, carts, orders, payments, coupons, delivery settings, reviews, and admin dashboard data.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server
- Redis
- JWT Authentication
- Google Authentication
- Stripe
- Cloudinary
- SignalR
- Hangfire
- FluentValidation
- AutoMapper
- Serilog
- Swagger / OpenAPI

## Main Features

### Authentication & Authorization

- User registration and login
- JWT access tokens
- Refresh tokens
- Role-based authorization
- Google login
- Password reset flow
- Resource-owner authorization for user-specific resources
- Authentication tokens handled through HTTP cookies

### Products & Catalog

- Product management
- Categories and brands
- Product images
- Product reviews
- Product filtering and pagination
- Redis caching for product/catalog data
- Product soft deletion
- Discount expiration cleanup

### Cart & Orders

- Cart and cart-item management
- Order creation and confirmation
- Order items and order summaries
- Multiple customer addresses
- Delivery methods and delivery zones
- Coupon validation and discounts
- Customer order history
- Admin order data

### Payments

- Stripe PaymentIntent integration
- Cash on delivery
- Payment method management
- Payment information associated with orders

### Background Jobs & Notifications

- Hangfire background processing
- Scheduled cleanup of expired coupons
- Scheduled cleanup of expired product discounts
- SignalR notifications for real-time updates

### Validation, Errors & Logging

- FluentValidation for request validation
- Centralized exception handling middleware
- Consistent API operation results
- Serilog logging to console, files, and Seq

## Project Structure

The solution is organized into separate layers:

```text
TechSouq-Backend/
│
├── TechSouq-API/
│   ├── Controllers/
│   ├── Extensions/
│   ├── Hubs/
│   ├── Middlewares/
│   ├── Policies/
│   └── Program.cs
│
├── TechSouq.Domian/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
│
├── TechSouq-Business-Layer/
│   ├── Dtos/
│   ├── Queries/
│   ├── Services/
│   ├── Validators/
│   ├── Mappings/
│   └── Helper/
│
└── TechSouq-DataLayer/
    ├── Data/
    ├── Queries/
    ├── Repositories/
    └── Migrations/
```

### Layer Responsibilities

- **API** — HTTP endpoints, authentication configuration, middleware, policies, SignalR hubs, and application startup.
- **Domain** — Core entities, enums, and repository contracts.
- **Application** — Business logic, DTOs, validation, services, queries, and mappings.
- **Infrastructure** — EF Core database access, repositories, queries, migrations, and external infrastructure services.

## Getting Started

### Requirements

Make sure you have:

- .NET 8 SDK
- SQL Server
- Redis

Optional depending on the features you want to run locally:

- Stripe account / test keys
- Cloudinary account
- Google OAuth credentials
- SMTP credentials
- Seq

### Clone

```bash
git clone https://github.com/Hosny-Ayman/TechSouq-Backend.git
cd TechSouq-Backend
```

### Configuration

The API reads its database and Redis connection settings from environment variables when available, with configuration values used as fallback.

You will need to configure the following before running the application:

- SQL Server connection string
- Redis connection
- JWT settings
- Stripe settings
- Cloudinary settings
- Google authentication settings
- SMTP settings if password recovery emails are enabled
- Seq URL if you want Seq logging

Do not commit real connection strings, API keys, JWT secrets, or other credentials.

### Database

The application runs Entity Framework Core migrations during startup.

You can also apply migrations manually with:

```bash
dotnet ef database update
```

### Run

From the solution directory:

```bash
dotnet run --project TechSouq-API/TechSouq.API.csproj
```

Once the API is running, Swagger is available from the URL shown by the ASP.NET Core development server.

## Related Projects

- TechSouq Client Storefront: https://github.com/Hosny-Ayman/TechSouq-Frontend
- TechSouq Admin Dashboard: https://github.com/Hosny-Ayman/TechSouq-Admin-Dashboard

## Notes

This repository contains the backend API only. The client storefront and admin dashboard are maintained in separate repositories.
=======
# TechSouq Backend API 🛒

A robust, highly scalable, and enterprise-grade secure e-commerce RESTful API built with **.NET 8** following **Clean Architecture** principles. This backend powers the TechSouq client store and the admin dashboard, providing secure payments, real-time notifications, and high-performance data delivery.

🚀 **Live API / Swagger:** [teckseq-api.runasp.net/swagger](https://teckseq-api.runasp.net/swagger/index.html)

## 🏗️ Architecture & Performance Optimization
* **Clean Architecture:** Strictly separated layers (Domain, Application, Infrastructure, API) to ensure decoupling, maintainability, and testing readiness.
* **Separation of Read/Write (CQRS-Lite):** Distinct interfaces for Queries (Reads) and Repositories (Writes) to optimize database interactions.
* **Data Structures Efficiency:** Strategic use of `Dictionary<TKey, TValue>` in complex operations (like `AddCartItems`) to achieve **O(1) time complexity** for lookups and manipulation, significantly reducing execution time.
* **Pagination & Data Shaping:** Implemented server-side pagination for all heavy endpoints (Products, Orders, Reviews) to minimize payload size, reduce DB load, and guarantee fast client rendering.
* **DTOs & AutoMapper:** Decoupling database models from API contracts to protect internal data structures.

## 🔐 Security, User Management & Validation
* **Authentication & Authorization:** JWT (JSON Web Tokens) with Role-Based Access Control (Admin vs. Customer) + **Google OAuth 2.0 Integration**.
* **Secure Cookie Transmission:** Tokens and sensitive state data are transmitted using **HttpOnly, Secure, and SameSite Cookies** to prevent XSS (Cross-Site Scripting) and CSRF attacks.
* **Resource Owner Authorization (IDOR Prevention):** Custom authorization handlers protecting endpoints, ensuring users can strictly access or modify only their personal data.
* **Restrictive CORS Policy:** API is securely locked down to accept requests *only* from predefined and trusted frontend origins (Vercel deployments), blocking unauthorized cross-origin requests.
* **Password Hashing:** Fully encrypted credentials using highly secure **BCrypt**.
* **Rate Limiting:** Custom IP-based rate limiting policies built into the pipeline to guard against DDoS, brute-force attacks, and spam.
* **Automatic Request Validation:** Integrated **FluentValidation** pipeline acting as a barrier to validate payloads before executing action methods.

## 🛠️ Core Tech Stack & Integrations

* **Framework:** .NET 8 / ASP.NET Core RESTful Web API
* **Database & ORM:** SQL Server, Entity Framework Core (with automated migrations execution on startup).
* **Caching Layer:** Redis Distributed Cache (`StackExchange.Redis`) for lightning-fast catalog retrieval.
* **Real-time Engine:** SignalR WebSockets providing instantaneous order updates.
* **Background Processing:** Hangfire executing daily scheduled Cron jobs for automated cleanup of expired coupons/discounts.
* **Payment Gateway:** Native Stripe Integration handling secure Payment Intents and verifying Stripe Signatures via Webhooks.
* **Media Management:** Cloudinary API integration for seamless multi-image cloud galleries storage.
* **Telemetry & Cloud Logging:** Serilog streaming logs live to **BetterStack Telemetry** for real-time monitoring.

## ✨ Advanced Engineering Features

* **Smart Transactional Media Rollback:** Logic that intercepts runtime data faults and automatically purges newly uploaded images from Cloudinary/Disk if the database transaction fails.
* **Hybrid Storage Architecture:** Hot-swappable file system logic that can alternate dynamically between **Cloudinary Storage** and **Local File System Hosting** (`wwwroot/ProductImages`).
* **Optimized E-Commerce Engine:** Complex conditional coupon validation, dynamic delivery zone cost calculations, and secure password recovery flow via SMTP.

## ⚙️ How to Run Locally

1. Clone the repository: `git clone https://github.com/Hosny-Ayman/TechSouq-Backend.git`
2. Update `appsettings.json` with your private keys:
   * SQL Server & Redis Connection Strings
   * Stripe API Keys & Webhook Secret
   * Cloudinary Configs & SMTP Credentials
   * JWT Secret Key & Google Client ID
3. Apply Entity Framework Migrations: `dotnet ef database update`
4. Run the application: `dotnet run`
>>>>>>> bfe721c4a7650c9c8647b7d76e120498386dd1ae
