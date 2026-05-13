**Overview**

This is a simple ASP.NET Core Web API application for managing Company and Product entities.
It provides RESTful CRUD operations using a layered clean architecture.

**Features along with their API Endpoints:**

*Product APIs*

Get all products                           : GET /ecommerce/Products/allproducts
Get product by ID                          : GET /ecommerce/Products/byproductid/{id}
Get product by name                        : GET /ecommerce/Products/byproductname/{name}
Get all products of a company by companyId : GET ecommerce/Products/sortedbycompany/5
Create a new product                       : POST /ecommerce/Products/createproduct
Update product (PUT)                       : PUT /ecommerce/Products/productupdate/{id}
Partially update product (PATCH)           : PATCH /ecommerce/Products/updateproduct/{id}
Delete product                             : DELETE /ecommerce/Products/deleteproduct/{id}

*Company APIs*

Get all companies                          : GET /ecommerce/Company/allcompany
Get company by ID                          : GET /ecommerce/Company/bycompanyid/{id}
Get company by Name                        : GET /ecommerce/Company/bycompanyname/{name}
Create a new company                       : POST /ecommerce/Company/createcompany
Update company (PUT)                       : PUT /ecommerce/Company/companyupdate/{id}
Partially update company (PATCH)           : PATCH /ecommerce/Company/updatecompany/{id}
Delete company                             : DELETE /ecommerce/Company/deletecompany/{id}

**Project Structure**

ECommerceApp/
 ├── Controllers/
 │     ├── ProductController.cs
 │     └── CompanyController.cs
 │
 ├── Data/
 │     ├── EcommerceContext.cs
 │
 ├── Services/
 │     ├── Interfaces/
 │     │      ├── IProductService.cs
 │     │      └── ICompanyService.cs
 │     ├── ProductService.cs
 │     └── CompanyService.cs
 │
 ├── Repositories/
 │     ├── Interfaces/
 │     │      ├── IProductRepository.cs
 │     │      └── ICompanyRepository.cs
 │     ├── ProductRepository.cs
 │     └── CompanyRepository.cs
 │
 ├── Models/
 │     ├── Product.cs
 │     └── Company.cs
 │
 ├── DTO/
 │     ├── ProductDTO.cs
 │     ├── CompanyDTO.cs
 │     ├── ProductPatchDTO.cs
 │     └── CompanyPatchDTO.cs
 └── Program.cs


**Architecture**

The application follows a layered architecture:
Controllers – Handle HTTP requests and responses
Services (IService + Implementation) – Contain business logic
Repositories (IRepository + Implementation) – Handle database operations
Models – Define entity structures

**How to Run**

Clone the repository
Update the connection string in appsettings.json
Run the project using:
dotnet run
Access Swagger to test the APIs.
