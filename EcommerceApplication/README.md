# Ecommerce Application API

## Overview

A robust ASP.NET Core Web API for managing an e-commerce platform. The application follows Clean Architecture principles with Repository, Service and Controller layers, JWT-based authentication and authorization, role-based access control, pagination, filtering, and secure user management using ASP.NET Core Identity.

## Features

### Product Management

* Create, update, delete, and retrieve products
* Associate products with categories and companies
* Search products by:

  * Price range
  * Company
  * Category
* View all products belonging to a specific company

### Company Management

* Create, update, delete, and retrieve companies
* Pagination support for company listings

### Authentication & Authorization

* JWT Access Token authentication
* Refresh Token implementation
* ASP.NET Core Identity integration
* Claims-based authorization
* Role-based access control

### User Roles

* **Admin**

  * Full access to management endpoints
  * Product, company, category, and order administration

* **Customer**

  * Access to customer-specific operations
  * Browse products

### Pagination

Efficient pagination implemented for:

* Product listing
* Company listing

### Security Features

* JWT Authentication
* Refresh Tokens
* Claims-based Authorization
* Role-based Access Control
* Identity User Management
* Secure API Endpoints

---

## Technology Stack

* ASP.NET Core Web API
* Entity Framework Core
* ASP.NET Core Identity
* JWT Authentication
* SQL Server
* LINQ
* Dependency Injection

## Project Structure

```text
EcommerceApplication
│
├── Controllers
│   ├── ProductController
│   ├── CompanyController
│   └── AuthController
│
├── Services
│   ├── Interfaces
│   ├── Product Services
│   ├── Company Services
│   └── Auth Services
│
├── Repositories
│   ├── Interfaces
│   ├── Product Repository
│   ├── Company Repository
│
├── Models
│   ├── ApplicationUser
│   ├── Product
│   ├── Company
│   ├── Category
│   ├── Order
│   └── OrderItem
│
├── Data
│   ├── Seeder
│   │   ├── RoleSeeder
│   ├── EcommerceContext
│
├── DTO
│   ├── AuthResponseDTO
│   ├── AuthResult
│   ├── CompanyDTO
│   ├── CompanyPatchDTO
│   ├── ProductDTO
│   ├── ProductPatchDTO
│   ├── ProductFilterDTO
│   ├── RefreshTokenRequestDTO
│   ├── RegisterDTO
│   ├── Login
│
└── Mapping
│   ├── MappingProfiles
```
## Authentication Flow

1. User registers using ASP.NET Core Identity.
2. User logs in with credentials.
3. API generates:

   * Access Token (JWT)
   * Refresh Token
4. Access Token is used to access protected endpoints.
5. Refresh Token is used to obtain a new Access Token when it expires.

---

## API Features

### Product Endpoints

* Get all products, search by company, category and price range
* Get product by ID
* Get product by name
* Create product
* Update product
* Patch (Update partially) product
* Delete product
* Sort by company


### Company Endpoints

* Get all companies
* Get company by id
* Get company by name
* Search company by name
* Search company by location
* Create company
* Update company
* Delete company
* Patch (Update partially) company

### Auth Endpoints

* Register
* Login
* RefreshToken
* Logout


## Future Enhancements

* Shopping Cart Module
* Product Reviews and Ratings
* Wishlist Functionality
* Payment Gateway Integration
* Email Notifications
* Order Tracking
