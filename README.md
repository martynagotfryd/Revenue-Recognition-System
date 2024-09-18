# Revenue Recognition System

## Project Requirements
[Project Requirements](/Project.pdf)

## Project Overview
The Revenue Recognition System is a REST API designed for large corporations to manage revenue recognition. This project focuses on solving complex revenue recognition issues, such as when to recognize revenue for products and services sold, applying various discount schemes, and tracking subscription-based sales.

### Features
- Client Management: Add, update, and remove clients (both individual and company clients).
- Software License Management: Manage the sales of software systems via upfront payments.
- Revenue Recognition: Automatically recognize revenue for contracts and payments in compliance with industry standards.
- Discount Management: Apply and track discounts for different products based on predefined time ranges and categories.
- Revenue Calculation: Calculate both current and predicted revenue, with currency exchange support.

## Installation
To set up and run the project locally, follow these steps:

1. Clone the repository:
```
git clone https://github.com/martynagotfryd/apbd.project.git
cd apbd.project
```

2. Install dependencies: Ensure you have .NET Core SDK installed, then run:
```
dotnet restore
```

3. Configure the database: Update the connection string in appsettings.json to point to your local SQL Server instance. Use Entity Framework to set up the database:
```
dotnet ef database update
```

4. Run the application:
```
dotnet run
```

5. Swagger: Navigate to https://localhost:5001/swagger to access the Swagger UI and test API endpoints.

## Usage
Once the application is running, you can manage clients, software licenses, contracts, and subscriptions via the exposed API endpoints. Authentication is required for all actions, and admin access is required for certain operations (e.g., editing or removing clients).

### API Endpoints
Here are some example API endpoints:

1. Client Management
- Add a client: `POST /api/clients`
- Update a client: `PUT /api/clients/{id}`
- Remove a client (soft delete): `DELETE /api/clients/{id}`

2. Contracts
- Create a contract: `POST /api/contracts`
- Pay for a contract: `POST /api/contracts/{id}/pay`

3. Revenue Calculation
- Calculate current revenue: `GET /api/revenue/current`
- Calculate predicted revenue: `GET /api/revenue/predicted`

## Testing
To run unit tests:

```
dotnet test
```

Unit tests focus on business logic for client management, contract creation, revenue recognition, and other core functionalities.

## Technologies Used
- .NET Core
-Entity Framework Core
- SQL Server
- Swagger for API documentation and testing
- XUnit for unit testing
- REST API architecture
