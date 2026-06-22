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
