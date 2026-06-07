-- Database creation script for Payroll Management System
-- This script creates the database, table, and inserts sample data

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'PayrollDb')
BEGIN
    CREATE DATABASE PayrollDb;
END
GO

USE PayrollDb;
GO

IF OBJECT_ID('Employees', 'U') IS NOT NULL
    DROP TABLE Employees;
GO

CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeNumber VARCHAR(20) NOT NULL UNIQUE,
    LastName NVARCHAR(50) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    MiddleName NVARCHAR(50) NULL,
    DateOfBirth DATE NOT NULL,
    DailyRate DECIMAL(18,2) NOT NULL,
    WorkingDays VARCHAR(10) NOT NULL
);
GO

-- Insert sample data
INSERT INTO Employees (EmployeeNumber, LastName, FirstName, MiddleName, DateOfBirth, DailyRate, WorkingDays)
VALUES
('JOH-12345-15JAN1990', 'John', 'Doe', 'Middle', '1990-01-15', 100.00, 'MWF'),
('SMI-67890-22FEB1985', 'Smith', 'Jane', 'Ann', '1985-02-22', 150.00, 'TTHS');
GO

-- Optional: Create a view for employee details
CREATE VIEW vw_EmployeeDetails AS
SELECT
    Id,
    EmployeeNumber,
    LastName,
    FirstName,
    MiddleName,
    DateOfBirth,
    DailyRate,
    WorkingDays
FROM Employees;
GO