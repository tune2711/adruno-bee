
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
    *   `TransactionsController` at `/api`.
    *   `GET /api/GET`: Retrieves all transactions.
    *   `GET /api/GET/{id}`: Retrieves a specific transaction by its primary key (`Id`).
    *   `GET /api/GET/id/{transactionId}`: Retrieves a specific transaction by its unique `TransactionId`.
    *   `POST /api/POST`: Allows the Arduino device to submit new transactions. It now correctly saves the `userName` provided in the request body.
*   **Frontend:**
    *   A simple HTML page (`wwwroot/index.html`) to display the transaction history in a table.
    *   The table includes a column for the transaction sender, labeled "Người Chuyển Khoản".

## Current Request

*   **User Goal:** Change request URL to be shorter and not include "/Transactions".
*   **AI Plan:**
    1.  **Stop the running application.**
    2.  **Update `Controllers/TransactionsController.cs`**: Change the routes to `/api/GET`, `/api/POST`, `/api/GET/{id}`, and `/api/GET/id/{transactionId}`.
    3.  **Update `wwwroot/index.html`**: Update the `fetch` URL to `/api/GET`.
    4.  **Update `blueprint.md`**: Document the new, shorter API endpoints.
    5.  **Restart the application.**
