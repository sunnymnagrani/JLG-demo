# JLG-demo
demo project using angular and dot net core web api

# Task Management Application

Task Management CRUD application built with an **ASP.NET Core Web API (.NET 10)** backend, a **PostgreSQL** database, and an **Angular (v21)** frontend. 

Key features include automated audit logs (`CreatedDate`/`UpdatedDate`), global exception handling, repository patterns, and automated unit testing suites.

---

## Tech Stack & Key Features

### Backend (.NET 10 Web API)
- **Framework:** ASP.NET Core Web API (.NET 10)
- **Database ORM:** Entity Framework Core Code-First with PostgreSQL
- **Architecture:** Repository Pattern decoupling logic with Interfaces (`ITaskDataRepository`)
- **Error Pipeline:** Integrated .NET 10 `IExceptionHandler` for uniform global JSON error reporting (404/500)

### Frontend (Angular v21)
- **Framework:** Angular 21 (Standalone Component architecture)
- **State Management:** Modern reactive streaming using `RxJS Observables`
- **Forms Layout:** Strict Type-safe Reactive Forms mapping custom interfaces (`Task`)

---

### Prerequisites
Make sure you have the following installed on your machine:
- [.NET 10 SDK](https://microsoft.com)
- [Node.js (v22+)](https://nodejs.org) & Angular CLI (`npm install -g @angular/cli`)
- [PostgreSQL Database Instance](https://postgresql.org)

---

## 📂 Installation & Database Settings

### 1. Database Configuration
Update the connection string inside `TaskManagementApplication/appsettings.json` to link your local PostgreSQL instance:

```json
"ConnectionStrings": {
  "DbConnection": "Host=localhost;Database=TaskManagementDb;Username=postgres;Password=your_password"
}
```

### 2. Backend Setup & Code-First Migrations
Navigate to your backend directory to install packages, apply migrations, and start the server:

```bash
cd TaskManagementApplication

# Restore packages & generate PostgreSQL schema updates
dotnet ef migrations add AddTimestampsToTasks
dotnet ef database update

# Run the API application
dotnet run
```
Once started, you can test using Swagger UI at: `https://localhost:7197/swagger`

### 3. Frontend Setup
Navigate to your Angular folder, install dependencies, and run the development pipeline:

```bash
cd task-management-frontend

# Install node dependencies
npm install

# Start the application locally
ng serve
```
Open your browser and navigate to `http://localhost:4200` to view the app.

---

## Running Automated Unit Tests (.NET 10)

The backend features an isolated unit testing configuration using **xUnit**, **Moq**, and the EF Core **InMemory Database** provider. To run the automated testing pipeline via your terminal:

```bash
# Navigate to the test suite project folder
cd TaskManagementApplication.Tests

# Execute the test runner execution
dotnet test
```

Alternatively, open the **Test Explorer** inside **Visual Studio** (`Test > Test Explorer`) and run all test profiles natively using the visual UI layout.

---
