# Payroll Management System

A simple payroll management system built with ASP.NET Core 8 Web API and ASP.NET Core MVC.

## Features

- **Backend**: ASP.NET Core 8 Web API with Entity Framework Core
- **Frontend**: ASP.NET Core MVC with Bootstrap UI
- **RESTful API**: CRUD operations for employees and payroll calculation
- **Database**: SQL Server with Entity Framework Core
- **Validation**: Form validation on both client and server sides

## Project Structure

- `/Backend/API` - ASP.NET Core Web API project
- `/Frontend/MVC` - ASP.NET Core MVC project
- `/Database.sql` - SQL script for database creation

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or use Docker)
- [Git](https://git-scm.com/)

## Setup Instructions

### 1. Clone the repository
```bash
git clone <repository-url>
cd PayrollManagementSystem
```

### 2. Configure the database

#### Option A: Using the provided SQL script
1. Open SQL Server Management Studio (SSMS)
2. Run the `Database.sql` script to create the database and tables
3. Update the connection string in `Backend/API/Api/appsettings.json` if needed

#### Option B: Using Entity Framework Core (if migrations were created)
1. Update the connection string in `Backend/API/Api/appsettings.json`
2. Run the following commands in the API project directory:
   ```bash
   cd Backend/API/Api
   dotnet ef database update
   ```

### 3. Configure the API

The API is configured to run on `https://localhost:7210` (and `http://localhost:5017`) by default.
The MVC web application is configured to call the API at `http://localhost:5017/` by default.
If you need to change these URLs:
- Update the API URL in `Backend/API/Api/Properties/launchSettings.json`
- Update the MVC application's API client configuration in `Frontend/MVC/Web/Program.cs`

### 4. Run the applications

> **Note**: You'll need to run both the API and the MVC web application. The API must be running before the web application can function properly.
> You can use two separate terminal windows, or run one process in the background.

#### Start the API
```bash
cd Backend/API/Api
dotnet run
```
The API will be available at `https://localhost:7210` and `http://localhost:5017` (or check the console output for the exact URLs).

#### Start the MVC web application
```bash
cd Frontend/MVC/Web
dotnet run
```
The web application will be available at `https://localhost:7099` and `http://localhost:5299` (or check the console output for the exact URLs).

### 5. Access the application

- Open your browser and navigate to `https://localhost:7099` (or `http://localhost:5299` for HTTP)
- Click on the "Employees" menu to view, create, edit, delete employees
- Use the "Compute Pay" menu to calculate take-home pay for employees

## API Endpoints

- `GET /api/employees` - Get all employees
- `GET /api/employees/{id}` - Get employee by ID
- `POST /api/employees` - Create a new employee
- `PUT /api/employees/{id}` - Update an existing employee
- `DELETE /api/employees/{id}` - Delete an employee
- `POST /api/employees/compute-pay?employeeId={id}` - Calculate take-home pay for an employee

## Database Schema

The `Employees` table contains the following columns:
- `Id` (int, primary key)
- `EmployeeNumber` (varchar(20), unique)
- `LastName` (nvarchar(50))
- `FirstName` (nvarchar(50))
- `MiddleName` (nvarchar(50))
- `DateOfBirth` (date)
- `DailyRate` (decimal(18,2))
- `WorkingDays` (varchar(10)) - Either "MWF" or "TTHS"

## Payroll Calculation

The take-home pay is calculated using the following rules:
1. Employee works only on scheduled days:
   - MWF: Monday, Wednesday, Friday (3 days per week)
   - TTHS: Tuesday, Thursday (2 days per week)
2. Employee receives twice the daily rate for every scheduled working day.
3. Employee receives an additional 100% of daily rate on their birthday (whether they worked or not).

Formula:
```
TakeHomePay = (NumberOfWorkingDays × DailyRate × 2) + BirthdayBonus
```

Where:
- NumberOfWorkingDays = 3 for MWF, 2 for TTHS
- BirthdayBonus = DailyRate if today is the employee's birthday (month and day match), otherwise 0

## Technologies Used

- **Backend**: ASP.NET Core 8, Entity Framework Core, AutoMapper
- **Frontend**: ASP.NET Core MVC, Bootstrap 5
- **Database**: SQL Server
- **Tools**: Git, .NET CLI

## Notes

- This is a simplified implementation suitable for learning and assessment purposes.
- In a production environment, consider adding:
  - Authentication and authorization
  - More comprehensive error handling
  - Logging
  - Unit tests
  - Docker support
  - Environment-specific configurations

## License

This project is for educational purposes only.
