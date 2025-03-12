# Warehouse Management System (WMS)

A traditional three-tier architecture WMS (Warehouse Management System) built with WinForms, Dapper, and Dependency Injection.

## Architecture

The application follows a traditional three-tier architecture:

1. **Presentation Layer (UI)**: WinForms application
2. **Business Logic Layer (BLL)**: Contains business logic and validation
3. **Data Access Layer (DAL)**: Uses Dapper for database access

## Features

- Product management
- Warehouse management
- Inventory management
- Inbound and outbound order processing (planned)
- Reporting (planned)

## Prerequisites

- .NET 6.0 or later
- SQL Server (local or remote)
- Visual Studio 2022 or later (recommended)

## Setup Instructions

### Database Setup

1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Run the SQL script located at `Database/CreateDatabase.sql`
   - This will create the WMS database and required tables
   - It will also insert sample data for testing

### Application Configuration

1. Open the solution in Visual Studio
2. In the `Program.cs` file, update the connection string to match your SQL Server configuration:

```csharp
string connectionString = "Server=YOUR_SERVER;Database=WMS;Trusted_Connection=True;";
```

### Build and Run

1. Build the solution
2. Run the WMS.UI project

## Project Structure

- **WMS.Model**: Contains entity classes
- **WMS.DAL**: Data Access Layer with repositories
- **WMS.BLL**: Business Logic Layer with services
- **WMS.UI**: Presentation Layer with WinForms

## Usage

1. **Product Management**: Add, edit, and delete products
2. **Warehouse Management**: Add, edit, and delete warehouses
3. **Inventory Management**: Manage product inventory across warehouses

## Dependencies

- Dapper: SQL object mapper
- System.Data.SqlClient: SQL Server data provider
- Microsoft.Extensions.DependencyInjection: Dependency injection container

## License

This project is licensed under the MIT License. 