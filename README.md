### This is my first ERP (Enterprise Resource Planning) web application, built entirely from scratch. It serves as a self-guided training project to prepare myself for a future professional role in software development. All changes and new branches are created using git commands


### ⚙️ Technologies & Concepts I'm Exploring
- **Programming Language**: C#

- **Framework**: ASP.NET Core MVC

- **Database**: Microsoft SQL Server (MSSQL)

- **Frontend**: Razor Views, HTML/CSS, JS

- **Architecture**: Micro-Services (in progress)

### ✅ Features

- [ ] User authentication with JWT tokens (in progress)
- [ ] Role-based authorization (Admin, Employee, etc.)
- [x] Employee management (CRUD operations)
- [ ] Department management
- [ ] Project assignment logic
- [ ] Logging system with custom middleware
- [ ] Audit trails for data changes
- [x] RESTful API endpoints
- [x] Client-side enhancements with JavaScript


### 📦 Projects Separation

- `ERP.sln`  
  Main solution file that aggregates all sub-projects.
1) <details>
    <summary>Employees/</summary>

    - `Employees.Api`
        - Exposes RESTful API endpoints for managing employee-related operations.
        - ASP.NET Core MVC web front-end (Controllers, Razor views, layout structure)
        - Mapping, wwwroot

    - `Employees.BackgroundServices`  
        - Contains methods for Database CleanUp

    - `Employees.Infrastructure`  
        - Responsible for data access logic (ADO.NET, SQL commands, database connections).

    - `Employees.Core/`  
       - Business logic layer: interfaces, services, and domain-driven logic.

    - `Employees.Domain`  
        - Containing Models

    - `Employees.Contracts`
        - Request and Response for each model for specified data access

    - `Employees.Shared`
        - Responsible for data type manipulation
    </details>
2) `Coming Soon...`

### 📝 License
This project is shared **for educational and review purposes only**.

You are allowed to:
- View and study the code
- Reference ideas for personal learning

You are **not allowed to**:
- Use, modify, or distribute this project for commercial or production purposes
- Copy or republish the code as your own

All rights remain with the original author.