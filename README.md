# E-commerce API

## Overview
Welcome to the E-commerce API built with ASP.NET 8. This API facilitates typical e-commerce operations such as managing products, categories, orders, users, and administrative tasks.

## Table of Contents
1. [Getting Started](#getting-started)
2. [Features](#features)
3. [API Endpoints](#api-endpoints)
    - [Account](#account)
    - [Admin](#admin)
    - [Cart](#cart)
    - [Category](#category)
    - [Product](#product)
    - [Order](#order)


## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server 


## Features

1. **JWT Authentication:** Secure your API endpoints using JSON Web Tokens for authentication.
2. **Generic Classes:** Utilize generic classes to write reusable code for various data types and scenarios.
3. **SQL Server:** Use SQL Server as the primary database for storing data.
4. **Entity Framework Core:** Employ Entity Framework Core as the ORM (Object-Relational Mapper) for database interactions.
5. **LINQ:** Leverage Language-Integrated Query (LINQ) for querying data from different data sources.
6. **DTOs:** Implement Data Transfer Objects (DTOs) to transfer data between different layers of the application.
7. **MailKit & MimeKit:** Use MailKit and MimeKit for sending and receiving emails within the application.
8. **Dependency Injection:** Implement Dependency Injection to manage class dependencies and improve code maintainability.
9. **DRY Principle:** Follow the "Don't Repeat Yourself" (DRY) principle to reduce duplication and enhance code readability.
10. **Repository Pattern:** Implement the Repository Pattern to separate data access logic from business logic and improve testability and maintainability.
11. **Clean Architecture:** Design the application following Clean Architecture principles to achieve separation of concerns and maintainability.
12. **Identity for Authentication:** Utilize ASP.NET Identity for user authentication and authorization.

These features collectively enable the E-commerce API to provide robust functionality while maintaining code quality, security, and scalability.


## API Endpoints

### Account

#### Register
- **Endpoint:** `/api/Account/Register`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "firstName": "string",
        "lastName": "string",
        "phone": "string",
        "email": "user@example.com",
        "userName": "string",
        "password": "string",
        "confirmPassword": "string"
    }
    ```
- **Response:**
    ```json
    {
        "token": "string"
    }
    ```

#### LogIn
- **Endpoint:** `/api/Account/LogIn`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "userName": "string",
        "password": "string"
    }
    ```
- **Response:**
    ```json
    {
        "token": "string"
    }
    ```

#### RefreshToken
- **Endpoint:** `/api/Account/RefreshToken`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "token": "string"
    }
    ```
- **Response:**
    ```json
    {
        "token": "string"
    }
    ```

#### Change Password
- **Endpoint:** `/api/Account/ChangePassword`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "currentPassword": "string",
        "newPassword": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Password changed successfully"
    }
    ```

#### LogOut
- **Endpoint:** `/api/Account/LogOut`
- **Method:** POST
- **Response:**
    ```json
    {
        "message": "Logged out successfully"
    }
    ```

### Admin

#### CreateRole
- **Endpoint:** `/api/Admin/CreateRole`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "roleName": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Role created successfully"
    }
    ```

#### AddRoleToUser
- **Endpoint:** `/api/Admin/AddRoleToUser`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "userName": "string",
        "roleName": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Role added to user successfully"
    }
    ```

#### SendEmail
- **Endpoint:** `/api/Admin/SendEmail`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "toEmail": "string",
        "subject": "string",
        "body": "string",
        "attachments": ["string"]
    }
    ```
- **Response:**
    ```json
    {
        "message": "Email sent successfully"
    }
    ```

#### WelcomeEmail
- **Endpoint:** `/api/Admin/WelcomeEmail`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "userName": "string",
        "email": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Welcome email sent successfully"
    }
    ```

#### RemoveRoleFromUser
- **Endpoint:** `/api/Admin/RemoveRoleFromUser`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "userName": "string",
        "roleName": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Role removed from user successfully"
    }
    ```

#### DeleteRole
- **Endpoint:** `/api/Admin/DeleteRole/{roleId}`
- **Method:** DELETE
- **Response:**
    ```json
    {
        "message": "Role deleted successfully"
    }
    ```

#### DeleteUser
- **Endpoint:** `/api/Admin/DeleteUser`
- **Method:** DELETE
- **Request Body:**
    ```json
    {
        "userId": "string"
    }
    ```
- **Response:**
    ```json
    {
        "message": "User deleted successfully"
    }
    ```

### Cart

#### GetCart
- **Endpoint:** `/api/Cart/GetCart`
- **Method:** GET
- **Response:**
    ```json
    {
        "items": [
            {
                "productId": "int",
                "quantity": "int",
                "price": "decimal"
            }
        ]
    }
    ```

#### RemoveProductFromCart
- **Endpoint:** `/api/Cart/RemoveProductFromCart`
- **Method:** DELETE
- **Request Body:**
    ```json
    {
        "productId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Product removed from cart successfully"
    }
    ```

### Category

#### GetAll
- **Endpoint:** `/api/Category/GetAll`
- **Method:** GET
- **Response:**
    ```json
    [
        {
            "id": "int",
            "name": "string",
            "description": "string"
        }
    ]
    ```

#### GetByID
- **Endpoint:** `/api/Category/GetByID/{id}`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string"
    }
    ```

#### GetByName
- **Endpoint:** `/api/Category/GetByName/{name}`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string"
    }
    ```

#### AddCategory
- **Endpoint:** `/api/Category/AddCategory`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "name": "string",
        "description": "string"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string"
    }
    ```

#### UpdateCategory
- **Endpoint:** `/api/Category/UpdateCategory`
- **Method:** PUT
- **Request Body:**
    ```json
    {
        "name": "string",
        "description": "string"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string"
    }
    ```

#### DeleteCategory
- **Endpoint:** `/api/Category/DeleteCategory`
- **Method:** DELETE
- **Request Body:**
    ```json
    {
        "categoryId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Category deleted successfully"
    }
    ```

### Product

#### GetAll
- **Endpoint:** `/api/Product/GetAll`
- **Method:** GET
- **Response:**
    ```json
    [
        {
            "id": "int",
            "name": "string",
            "description": "string",
            "price": "decimal",
            "categoryId": "int"
        }
    ]
    ```

#### GetByID
- **Endpoint:** `/api/Product/GetByID/{id}`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```

#### GetByName
- **Endpoint:** `/api/Product/GetByName`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```

#### AddProduct
- **Endpoint:** `/api/Product/AddProduct`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "name": "string",
        "description": "string",
        "photo": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string",
        "photo": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```

#### AddProductToCart
- **Endpoint:** `/api/Product/AddProductToCart`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "productId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Product added to cart successfully"
    }
    ```

#### UpdateProduct
- **Endpoint:** `/api/Product/UpdateProduct`
- **Method:** PUT
- **Request Body:**
    ```json
    {
        "name": "string",
        "description": "string",
        "photo": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string",
        "description": "string",
        "photo": "string",
        "price": "decimal",
        "categoryId": "int"
    }
    ```

#### DeleteProduct
- **Endpoint:** `/api/Product/DeleteProduct`
- **Method:** DELETE
- **Request Body:**
    ```json
    {
        "productId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Product deleted successfully"
    }
    ```

### Order

#### GetAll
- **Endpoint:** `/api/Order/GetAll`
- **Method:** GET
- **Response:**
    ```json
    [
        {
            "id": "int",
            "userId": "int",
            "total": "decimal",
            "status": "string",
            "createdDate": "datetime"
        }
    ]
    ```

#### GetOrderbyID
- **Endpoint:** `/api/Order/GetOrderbyID/{id}`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "userId": "int",
        "total": "decimal",
        "status": "string",
        "createdDate": "datetime"
    }
    ```

#### MakeOrder
- **Endpoint:** `/api/Order/MakeOrder`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "fullName": "string",
        "address": "string",
        "phone": "string"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "userId": "int",
        "total": "decimal",
        "status": "string",
        "createdDate": "datetime"
    }
    ```

#### DeleteOrder
- **Endpoint:** `/api/Order/DeleteOrder`
- **Method:** DELETE
- **Request Body:**
    ```json
    {
        "orderId": "int"
    }
    ```
- **Response:**
    ```json
    {
        "message": "Order deleted successfully"
    }
    ```

