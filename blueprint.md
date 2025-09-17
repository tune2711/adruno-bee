
# Blueprint

## Overview

This document outlines the structure, design, and features of the .NET web application. It serves as a single source of truth for the project's evolution.

The application is a backend service for a transaction notification device. It provides a web interface to view transaction data received from an Arduino device with a 4G SIM module.

## Project Outline

*   **Initial State:** A standard .NET 8 Web App template.
*   **Database Setup:**
    *   Entity Framework Core with SQLite.
    *   `ApplicationDbContext` to manage data.
    *   `AppUsers` table (from initial setup).
    *   `Transactions` table to store transaction data with the following columns: `Id`, `Amount`, `Description`, `Bank`, `Timestamp`, `UserName`.
*   **API:**
    *   `TransactionsController` at `/api/transactions`.
    *   `GET /api/transactions`: Retrieves all transactions.
    *   `GET /api/transactions/{id}`: Retrieves a specific transaction.
    *   `POST /api/transactions`: Allows the Arduino device to submit new transactions. It now correctly saves the `userName` provided in the request body.
*   **Frontend:**
    *   A simple HTML page (`wwwroot/index.html`) to display the transaction history in a table.
    *   The table includes a column for the transaction sender, labeled "Người Chuyển Khoản".

## Current Request

*   **User Goal:** Generate a unique transaction ID for each new transaction with the format `ddMMyyyyxxxxx`.
*   **AI Plan:**
    1.  **Update `blueprint.md`:** Document the new plan.
    2.  **Update `Models/Transaction.cs`**: Add a `TransactionId` string property to the `Transaction` model.
    3.  **Create Migration**: Run `dotnet ef migrations add AddTransactionIdToTransaction` to create the database migration.
    4.  **Update Database**: Run `dotnet ef database update` to apply the migration.
    5.  **Update `TransactionsController.cs`**: In the `PostTransaction` method, generate the unique `TransactionId` in the format `ddMMyyyy` followed by 5 random alphanumeric characters and assign it to the new transaction.
    6.  **Update `wwwroot/index.html`**: Add a "Mã Giao Dịch" column to the table to display the new `TransactionId`.
