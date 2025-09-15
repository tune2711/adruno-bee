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
    *   `Transactions` table to store transaction data.
*   **API:**
    *   `TransactionsController` at `/api/transactions`.
    *   `GET /api/transactions`: Retrieves all transactions.
    *   `GET /api/transactions/{id}`: Retrieves a specific transaction.
    *   `POST /api/transactions`: Allows the Arduino device to submit new transactions.

## Current Request

*   **User Goal:** Allow an Arduino-based device to save transaction information to the backend.
*   **AI Plan:**
    1.  **Create `Transaction` Model:** Define the data structure for a transaction in `Models/Transaction.cs`.
    2.  **Update `ApplicationDbContext`:** Add a `DbSet<Transaction>` to the context to manage the `Transactions` table.
    3.  **Create Migration:** Generate a new database migration (`AddTransactionModel`) to update the schema.
    4.  **Apply Migration:** Update the database with the new `Transactions` table.
    5.  **Create `TransactionsController`:** Implement API endpoints to handle creating and retrieving transactions.
    6.  **Update `blueprint.md`:** Document the new features and the plan.
