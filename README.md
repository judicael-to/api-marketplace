# Marketplace API

## Overview
This project provides a RESTful API for a marketplace platform, allowing users to manage categories, products, and inventory transactions. The API is built using OpenAPI 3.0 specifications and is designed to serve as the backend for marketplace operations.

## API Documentation
The API documentation is available through Swagger UI at:
```
http://localhost:5080/swagger
```

## Features
The API provides endpoints for managing:

### Categories
- `GET /api/Categories` - Retrieve all categories
- `POST /api/Categories` - Create a new category
- `GET /api/Categories/{id}` - Get a specific category by ID
- `PUT /api/Categories/{id}` - Update a category
- `DELETE /api/Categories/{id}` - Delete a category

### Inventory Transactions
- `GET /api/InventoryTransactions` - Retrieve all inventory transactions
- `POST /api/InventoryTransactions` - Create a new inventory transaction
- `GET /api/InventoryTransactions/{id}` - Get a specific transaction by ID
- `GET /api/InventoryTransactions/product/{productId}` - Get transactions for a specific product

### Products
- `GET /api/Products` - Retrieve all products
- `POST /api/Products` - Create a new product
- `GET /api/Products/{id}` - Get a specific product by ID
- `PUT /api/Products/{id}` - Edit a specific product by ID
- `DELETE /api/Products/{id}` - Delete a product
- `GET /api/Products/search` - Get products by query, categoryId, minPrice, maxPrice, or inStock

### Reports

- `GET /api/Reports/low-stock` - Retrieve products with low stock levels
- `GET /api/Reports/inventory-value` - Retrieve total inventory value report
- `GET /api/Reports/transactions-summary` - Retrieve summary of inventory transactions
- `GET /api/Reports/dashboard` - Retrieve dashboard data for the marketplace

### Suppliers

- `GET /api/Suppliers` - Retrieve all suppliers
- `POST /api/Suppliers` - Create a new supplier
- `GET /api/Suppliers/{id}` - Get a specific supplier by ID
- `PUT /api/Suppliers/{id}` - Update a supplier
- `DELETE /api/Suppliers/{id}` - Delete a supplier


## Data Models
The API uses the following data models:

### Category

- **id** - Unique identifier
- **name** - Category name
- **description** - Category description
- **products** - List of products in the category

## Technical Specifications
- **API Version**: v1
- **OpenAPI Specification**: 3.0
- **Server**: Runs locally on port 5080

## Getting Started

### Prerequisites
- .NET Core runtime (version required depends on implementation)
- Database system (configuration details in appsettings.json)

### Running the API
1. Clone the repository
2. Navigate to the project directory
3. Run the application:
   ```
   dotnet run
   ```
4. Access the Swagger UI documentation at http://localhost:5080/swagger

### Configuration
Configuration settings can be modified in the `appsettings.json` file. Update database connection strings and other settings as needed.

## Development
To extend or modify this API:
1. Use the Swagger UI to understand the existing endpoints
2. Implement new controllers or actions as needed
3. Update the OpenAPI specification if adding new endpoints

## Authentication
(Add details about authentication methods if implemented in your API)

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
For questions or support, please [contact me](judicaelto@protonmail.com).
