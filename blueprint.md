# Blueprint

## Overview

This document outlines the project structure, features, and implementation details of the user management application. The application uses a .NET backend with JWT-based authentication to provide secure access to its API endpoints.

## Project Details

*   **Backend:** ASP.NET Core Web API
*   **Database:** SQLite (using Entity Framework Core)
*   **Authentication:** JSON Web Tokens (JWT)

### Authentication & Authorization

The application uses a role-based authorization model.

1.  **Login (`POST /api/Login`):** Users authenticate by providing their email and password. Upon successful login, the API generates and returns a JWT.
2.  **JWT:** The token contains claims for the user's ID, email, and role. It is valid for 120 minutes.
3.  **Authorization:** The JWT must be included in the `Authorization` header (`Bearer <token>`) for all protected API calls. Access to certain endpoints is restricted by role.

### API Endpoints

#### Public Endpoints

*   **`POST /api/Login`**: Authenticates a user and returns a JWT.
*   **`POST /api/Users`**: Registers a new user. All new users are automatically assigned the role "người dùng".

#### Admin-Only Endpoints

*These endpoints require a valid JWT with the "admin" role.*

*   **`GET /api/Users`**: Retrieves a list of all users.
*   **`PUT /api/Users/{id}/role`**: Updates a specific user's role. Requires a JSON body with the `role` property (e.g., `{ "role": "manager" }`).
*   **`DELETE /api/Users/{id}`**: Deletes a user.

### Data Models

*   **`AppUser`**
    *   `Id` (int, Primary Key)
    *   `Email` (string, Required)
    *   `Password` (string, Required) - *Security Note: Passwords are in plaintext. Hashing is required for production.*
    *   `Role` (string, Required)

## Current Request: Admin-Only Role Editing

**Objective:** Restrict the ability to edit user roles to administrators only.

**Plan & Steps Executed:**

1.  **Install NuGet Package:** Added `Microsoft.AspNetCore.Authentication.JwtBearer` for JWT support.
2.  **Configure JWT:** Added JWT issuer, audience, and a secret key to `appsettings.json`.
3.  **Register Authentication Services:** Configured the application to use JWT authentication in `Program.cs`.
4.  **Update Login Controller:** Modified `LoginController` to generate and return a JWT upon successful login. The token includes the user's role claim.
5.  **Secure Users Controller:**
    *   Applied `[Authorize(Roles = "admin")]` attribute to `GET /api/Users`, `DELETE /api/Users/{id}`, and the newly created `PUT /api/Users/{id}/role` endpoints.
    *   Created a dedicated `UpdateUserRoleRequest` model and a specific `PUT /api/Users/{id}/role` route to prevent mass assignment vulnerabilities and ensure only the role is updated.
6.  **Update Blueprint:** This `blueprint.md` file was updated to reflect the new authentication and authorization flow.

**Outcome:** The user role editing functionality is now securely restricted to administrators using a JWT-based authentication and authorization system.
