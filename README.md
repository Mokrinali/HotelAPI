==============================
🏨 HotelBooking API - README
==============================

A secure and scalable ASP.NET Core Web API for managing hotel bookings, rooms, users, and authentication. It includes JWT authentication, role-based authorization, and Swagger integration for easy API testing.

------------------------------
🔧 Technologies Used
------------------------------
- ASP.NET Core 8
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- Swagger / OpenAPI
- CORS

------------------------------
📦 Project Setup
------------------------------

1. Clone the Repository:
   git clone <your-repo-url>
   cd HotelAPiV1/HotelAPiV1

2. Update Connection String:
   Edit appsettings.json and update the DefaultConnection:
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=HotelBookingDB;Integrated Security=true"
   }

3. Apply Migrations:
   dotnet ef database update

4. Run the Application:
   dotnet run

App will be hosted on https://localhost:7079 (or your configured port).

------------------------------
🔐 Authentication
------------------------------
The API uses JWT Bearer Tokens for authentication. Roles supported:
- Admin
- Manager
- User

Swagger is configured to accept JWTs via the "Authorize" button.

------------------------------
📂 Folder Structure
------------------------------
HotelAPiV1/
├── Data/               # EF Core DbContext and seeders
├── Models/             # Entity and DTO models
├── Repositories/       # Data access layer
├── Services/           # Business logic layer
├── Controllers/        # API endpoints
├── appsettings.json    # Config including JWT and connection string
├── Program.cs          # Main entry with DI and middleware config

------------------------------
📄 API Features
------------------------------
- User Registration & Login
- Hotel and Room Management
- Booking System
- Role-based Access Control
- Swagger UI at /swagger

------------------------------
✅ Sample JWT Config
------------------------------
"Jwt": {
  "Secret": "your-super-secure-long-secret",
  "Issuer": "https://localhost:7079",
  "Audience": "https://localhost:7079"
}

------------------------------
🛠 Development Notes
------------------------------
- Built with layered architecture (Controller → Service → Repository)
- Uses Dependency Injection for modular and testable components
- Supports CORS for frontend-backend communication

==============================
👥 Test Users
==============================

1. Admin User
   - Email: admin@example.com
   - Password: Admin@123
   - Role: Admin
   - Full Name: System Administrator

2. Regular User
   - Email: user@example.com
   - Password: User@123
   - Role: User
   - Full Name: System User
"""

