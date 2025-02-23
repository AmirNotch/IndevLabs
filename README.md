# JWT Authentication Example with ASP.NET Core

This project demonstrates how to implement JWT (JSON Web Token) authentication in an ASP.NET Core Web API. It includes user authentication, token generation, and protected endpoints.

## Features

- **JWT Authentication**: Secure your API endpoints using JWT tokens.
- **Token Generation**: Generate JWT tokens for authenticated users.
- **Protected Endpoints**: Restrict access to specific endpoints using the `[Authorize]` attribute.
- **Swagger Integration**: Test your API endpoints using Swagger UI with built-in JWT support.
- **Environment Configuration**: Store sensitive data like JWT keys and user credentials in environment variables or `appsettings.json`.

## Project Structure
- **Controllers: Contains API controllers (e.g., AuthController, WinesController).
- **Services: Contains business logic (e.g., AuthService, JwtService).
- **Models: Contains data models (e.g., Wine, User).
- **Extensions: Contains extension methods (e.g., database migration).
- **Validation: Contains validation logic (e.g., ValidIntAttribute).
- **Mappings: Contains AutoMapper profiles.

## What I Did
**Implemented JWT Authentication:

- **Configured JWT settings in appsettings.json.
- **Added JWT authentication middleware in Program.cs.
- **Generated JWT tokens using JwtService.

**Created Authentication Endpoints:

- **Added a login endpoint to authenticate users and return JWT tokens.
- **Stored user credentials (username and hashed password) in appsettings.json.

**Protected Endpoints:

- **Used the [Authorize] attribute to restrict access to specific endpoints.
- **Added role-based authorization (e.g., [Authorize(Roles = "Admin")]).

**Integrated Swagger:

- **Configured Swagger to support JWT authentication.
- **Added a "Authorize" button in Swagger UI to test protected endpoints.

**Added Logging:

- **Configured Serilog for logging to a file and console.
- **Added HTTP logging for development environments.

**Environment Configuration:

- **Stored sensitive data (e.g., JWT key, user credentials) in appsettings.json.

**Database Migrations:

- **Configured Entity Framework Core for database migrations (if applicable).

**Validation:

- **Added FluentValidation for request validation.
- **Created custom validation attributes (e.g., ValidIntAttribute).

**AutoMapper:

- **Configured AutoMapper for mapping between DTOs and entities.

**CORS:

- **Configured CORS to allow requests from any origin (for development purposes).


P.S
/api/public/login 

**body
{
  "username": "admin",
  "password": "mysecurepassword"
}
