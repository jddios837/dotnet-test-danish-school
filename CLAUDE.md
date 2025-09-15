# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a .NET 8 Clean Architecture project implementing a Customer/Lead Image Upload feature. The project follows Clean Architecture principles with separation of concerns across three main layers.

## Architecture

The solution uses Clean Architecture with the following structure:

- **Malte.Clean.API** - Web API layer (Controllers, Middleware, DTOs)
- **Malte.Clean.Domain** - Domain layer (Entities, Use Cases, Interfaces)
- **Malte.Clean.Data** - Infrastructure layer (Repositories, JSON Storage)

Dependencies flow inward: API → Domain ← Data, ensuring the domain layer remains independent of external concerns.

## Common Commands

### Build and Run
```bash
# Build the solution
dotnet build

# Run the API (launches Swagger UI at https://localhost:7159)
dotnet run --project src/Malte.Clean.API

# Run with HTTP only
dotnet run --project src/Malte.Clean.API --launch-profile http
```

### Development
```bash
# Restore packages
dotnet restore

# Clean build
dotnet clean && dotnet build

# Run tests (when test projects are added)
dotnet test
```

## Key Features

The main feature being implemented is a Customer/Lead Image Upload system with:
- Maximum 10 images per customer/lead
- Base64 image storage in JSON files
- RESTful API endpoints for upload, list, and delete operations
- Clean Architecture patterns with proper separation of concerns

## Data Storage

Currently uses JSON file storage for rapid prototyping. The architecture is designed to easily migrate to a database solution in the future while maintaining the same domain logic and API contracts.

## API Endpoints (Planned)

```
POST   /api/customers/{customerId}/images     # Upload images
GET    /api/customers/{customerId}/images     # List customer images
DELETE /api/customers/{customerId}/images/{imageId} # Delete image
GET    /api/customers/{customerId}            # Get customer details
```

## Development Notes

- Uses .NET 8 with nullable reference types enabled
- Swagger UI available at /swagger in development
- Project follows SOLID principles and Clean Architecture patterns
- Business rules (like 10-image limit) are enforced in the domain layer
- JSON storage implementation in Data layer can be swapped for database without affecting domain logic