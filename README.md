# Getting Started

## Prerequisites

Before running the application, ensure the following are installed:

* .NET SDK 8.0 or later
* SQL Server or SQL Server Express
* Visual Studio 2022 (recommended)
* Git

## Clone the Repository

In bash:
git clone https://github.com/SJP02/EcommerceApplication.git
cd EcommerceApplication

## Configure the Database

Create `appsettings.json` and fill in the following code along side update the connection string according to your SQL Server configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<your-localhost-name>;Database=<your-db-name>;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

## Configure JWT Settings

Update the JWT settings in `appsettings.json`.

```json
{
  "Jwt": {
    "Key": "<Your-Secret-Key>",
    "Issuer": "EcommerceApplication",
    "Audience": "EcommerceApplicationUsers",
    "AccessTokenExpirationMinutes": 30,
    "RefreshTokenExpirationDays": 7
  }
}
## Apply Database Migrations

Open the Package Manager Console or terminal and run:

```bash
dotnet ef database update
```

This will create the database and apply all Entity Framework Core migrations.

If migrations are not available, generate them first:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

---

## Restore Dependencies

```bash
dotnet restore
```

---

## Build the Application

```bash
dotnet build
```

---

## Run the Application

```bash
dotnet run
```

The API will start and display the local URL, for example:

```text
https://localhost:5289
http://localhost:5289
```

---

## API Documentation

If Swagger is enabled, navigate to:

```text
https://localhost:5289/swagger
````

Swagger provides an interactive interface to test API endpoints.

---

## Default User Roles

The application supports the following roles:

### Admin

* Full access to all endpoints
* Product management
* Company management
* User administration

### Customer

* Browse products,company

---

## Authentication Workflow

### Register

Create a user account using the registration endpoint.

### Login

Authenticate using email and password.

Successful authentication returns:

```json
{
  "accessToken": "jwt-token",
  "refreshToken": "refresh-token"
}
```

### Authorize Requests

Include the access token in the Authorization header:

```http
Authorization: Bearer <access_token>
```

### Refresh Token

Use the refresh token endpoint to obtain a new access token when the current token expires.

---

## Running Tests

Navigate to the test project directory and run:

```bash
dotnet test
```

---

## Troubleshooting

### Database Connection Error

Verify:

* SQL Server is running
* Connection string is correct
* Database permissions are configured properly

### Migration Errors

Ensure Entity Framework tools are installed:

```bash
dotnet tool install --global dotnet-ef
```

Verify installation:

```bash
dotnet ef --version
```

### Authentication Issues

Check:

* JWT secret key configuration
* Issuer and Audience values
* Token expiration settings

---

## Project Architecture

The application follows a layered architecture:

```text
Controllers
    ↓
Services
    ↓
Repositories
    ↓
Entity Framework Core
    ↓
SQL Server
```

This separation improves maintainability, testability, and scalability.
