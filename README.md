# Service Request Management System

This is a Full-Stack .NET 8 Web API and React application for managing technical service requests, developed as part of a technical assignment.

## Technologies Used
- **Backend:** .NET 8, ASP.NET Core Web API, Entity Framework Core, SQLite, N-tier Architecture
- **Frontend:** React, Vite, TypeScript, TailwindCSS, Axios, React Router
- **Testing:** xUnit, Moq
- **Others:** Swagger for API documentation

## Architecture
The backend follows a standard N-Tier architecture mirroring the provided examples:
- **Entities:** Contains core models and DTOs.
- **Core:** Contains generic repositories, standard result structures (SuccessDataResult, ErrorResult, etc.), and shared utilities.
- **DataAccess:** Entity Framework Core implementation, Context configuration, and migrations.
- **Business:** Handles the business logic, implementing operations like adding "New" status automatically.
- **Business.Tests:** Contains xUnit unit tests verifying core business logic.
- **WebAPI:** Controllers, Swagger config, Dependency Injection.

## Features
- **Create Request:** Create new service requests with device and customer information.
- **List Requests:** View all service requests.
- **Search & Filter:** Search requests by customer or device name, and filter by status.
- **Update Status:** Change the status of a request easily from the list.
- **Delete Request:** Remove requests.
- **Premium Aesthetics:** TailwindCSS is used for a rich, vibrant, modern user interface.
- **Unit Testing:** Business rules are tested automatically using xUnit and Moq.

## How to Run

### Backend
1. Open terminal and navigate to the root directory `ServiceRequestManagement`.
2. Apply database migrations (SQLite database will be created automatically):
   ```bash
   dotnet ef database update --project DataAccess/DataAccess.csproj --startup-project WebAPI/WebAPI.csproj
   ```
3. Run the API:
   ```bash
   cd WebAPI
   dotnet run
   ```
   The API will be available at `http://localhost:5000` (or the port specified in your launchSettings.json).
   Swagger UI is available at `/swagger`.

### Frontend
1. Open a new terminal and navigate to `ServiceRequestManagement/frontend`.
2. Install dependencies:
   ```bash
   npm install
   ```
3. Run the development server:
   ```bash
   npm run dev
   ```
   The React application will be available at `http://localhost:5173`.

### Testing
1. Open a terminal and navigate to the root directory `ServiceRequestManagement`.
2. Run the tests using the .NET CLI:
   ```bash
   cd Business.Tests
   dotnet test
   ```
   This will execute the xUnit tests written for the Business layer, validating the core business rules using Moq.
