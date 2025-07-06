# 🛠️ Product API (.NET 8) — Code Assessment

This is a RESTful Product Management API built with **ASP.NET Core 8**, designed as part of a backend coding. The API supports standard CRUD operations, manages stock, and is designed to scale across distributed environments with a 6-digit unique Product ID generator.

## 🚀 Features

- ✅ Create, Retrieve, Update, and Delete Products
- 🔁 Increment / Decrement product stock using dedicated endpoints
- 🆔 Auto-generated 6-digit unique Product ID (collision-safe)
- 🔐 Partial field updates (only name, description, price — not stock)
- 🧪 Unit tests using xUnit + FluentAssertions
- 💾 EF Core Code-First + Migrations
- 📚 Swagger UI available for endpoint testing

## 📁 Project Structure

ProductsAPI/                          # Root folder of the ASP.NET Core Web API project
├── Connected Services/               # (Auto-generated) External services referenced in the project
├── Dependencies/                     # NuGet packages and project references
├── Properties/                       # Project metadata and launch settings
├── Controllers/                      # Defines HTTP endpoints — entry point for API calls
│   └── ProductsController.cs         # Handles product CRUD and stock operations
├── Data/                             # Entity models and DbContext for EF Core
│   ├── AppDBContext.cs               # EF Core context: manages database operations
│   └── Products.cs                   # EF entity representing a Product
├── Migrations/                       # Auto-generated EF Core migration files (schema versions)
├── Models/                           # Request/response DTOs used between client and server
│   ├── ProductRequest.cs             # Incoming payload for create/update product
│   └── ProductResponse.cs            # Outgoing structure returned in API responses
├── Services/                         # Business logic layer — handles actual operations
│   ├── IProductServices.cs           # Interface defining service contract
│   └── ProductService.cs             # Implementation of product logic (create, update, stock, etc.)
├── Validator/                        # Custom validation logic for incoming data
│   └── ProductRequestValidator.cs    # Validates fields in ProductRequest (lengths, nulls, price, etc.)
├── appsettings.json                  # App configuration (e.g., connection strings, maxStock settings)
├── NLog.config                       # Logging configuration (file/console targets, log levels)
├── Program.cs                        # Main entry point that bootstraps and runs the web app
└── README.md                         # Project overview, setup steps, and documentation

## 🏗️ Setup Instructions

1. Clone the Repository
	git clone https://github.com/your-username/ProductAPI.git
	cd ProductAPI
2. Apply EF Core Migrations
    dotnet ef database update
3. Run the API
	dotnet run
4. Access Swagger UI
   Open your browser and navigate to `http://localhost:5000/swagger` to test endpoints interactively.

## 🧪 Running Unit Tests

	cd Tests/
	dotnet test

Covers:
- Product creation
- Field-specific updates (ignores 0 and empty fields)
- Stock increment / decrement

## ✅ Checklist

- [x] REST endpoints for product CRUD
- [x] Stock-specific endpoints implemented
- [x] 6-digit unique Product ID generation
- [x] Clean partial update logic (price > 0, no empty strings)
- [x] EF Core with Code First Migrations
- [x] Unit testing with edge case coverage
- [x] Swagger integrated
- [x] Complete run and build documentation

## 👤 Author

Abhilash
📍 Bengaluru, India
💻 Backend .NET Developer
For any questions or feedback, feel free to reach out.


