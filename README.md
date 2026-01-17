# Personal Finance Manager

An ASP.NET MVC application for managing personal finances.  
The application allows users to create accounts, record transactions, and track financial data using a SQL Server database.

---

## Requirements

Make sure the following are installed before running the application:

- Visual Studio (with ASP.NET and web development workload)
- .NET SDK 8
- SQL Server
- SQL Server Management Studio (SSMS)

---

## Steps to Run the Application

### 1. Create the Database
Inside SQL Server, create a database named: PersonalFinanceDB



---

### 2. Apply Entity Framework Migrations
Inside Visual Studio.

1. Open **Package Manager Console**
2. Select the correct startup project
3. Run the command: Update-Database

---

### 3. Run the Database Script
Copy and run the entire SQL script located in the **DB.script** folder inside the `PersonalFinanceDB` project.

---

### 4. Run the Application
Set the MVC project as the startup project and run the application.
---

