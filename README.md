# E-commerce API

## Overview
Welcome to the E-commerce API built with ASP.NET 8. This API facilitates typical e-commerce operations such as managing products, categories, orders, and users.

## Table of Contents
1. [Getting Started](#getting-started)
2. [API Endpoints](#api-endpoints)
    - [Authentication](#authentication)
    - [Products](#products)
    - [Categories](#categories)
    - [Orders](#orders)
    - [Users](#users)

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server (or any preferred database)

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/ecommerce-api.git
    cd ecommerce-api
    ```

2. Restore the dependencies:
    ```bash
    dotnet restore
    ```

3. Update the database connection string in `appsettings.json`:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
    }
    ```

4. Apply database migrations:
    ```bash
    dotnet ef database update
    ```

5. Run the application:
    ```bash
    dotnet run
    ```

The API should now be running on `http://localhost:5000`.

## API Endpoints

### Authentication
#### Register
- **Endpoint:** `/api/auth/register`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "username": "string",
        "email": "string",
        "password": "string"
    }
    ```
- **Response:**
    ```json
    {
        "token": "string"
    }
    ```

#### Login
- **Endpoint:** `/api/auth/login`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "email": "string",
        "password": "string"
    }
    ```
- **Response:**
    ```json
    {
        "token": "string"
    }
    ```

### Products
#### Get All Products
- **Endpoint:** `/api/products`
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

#### Create a Product
- **Endpoint:** `/api/products`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "name": "string",
        "description": "string",
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
        "price": "decimal",
        "categoryId": "int"
    }
    ```

### Categories
#### Get All Categories
- **Endpoint:** `/api/categories`
- **Method:** GET
- **Response:**
    ```json
    [
        {
            "id": "int",
            "name": "string"
        }
    ]
    ```

#### Create a Category
- **Endpoint:** `/api/categories`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "name": "string"
    }
    ```
- **Response:**
    ```json
    {
        "id": "int",
        "name": "string"
    }
    ```

### Orders
#### Get User Orders
- **Endpoint:** `/api/orders`
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

#### Create an Order
- **Endpoint:** `/api/orders`
- **Method:** POST
- **Request Body:**
    ```json
    {
        "products": [
            {
                "productId": "int",
                "quantity": "int"
            }
        ]
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

### Users
#### Get User Profile
- **Endpoint:** `/api/users/profile`
- **Method:** GET
- **Response:**
    ```json
    {
        "id": "int",
        "username": "string",
        "email": "string",
        "createdDate": "datetime"
    }
    ```


