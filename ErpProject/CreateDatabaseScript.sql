--Database creation
IF NOT EXISTS( SELECT name FROM sys.databases WHERE name = N'ErpNewDatabase')
    BEGIN
        CREATE DATABASE [ErpNewDatabase]
        PRINT 'ErpDatabase Created';
    END

    ELSE

    BEGIN
        PRINT 'ErpDatabase already exists';
    END
GO

--Using the created Database
USE [ErpNewDatabase]
GO

--Creation of Employees Table
IF OBJECT_ID('dbo.Employees', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Employees
    (
        EmployeeId INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Email NVARCHAR(50) NOT NULL,
        Age NVARCHAR(3) NOT NULL,
        DateOfBirth DATE NULL,
        Nationality NVARCHAR(20) NOT NULL,
        Gender NVARCHAR(10) NOT NULL,
        PhoneNumber NVARCHAR(13) NOT NULL,
        Mime NVARCHAR(max) NULL,
        Photograph VARBINARY(max) NULL,
        IsCompleted BIT NOT NULL,
        CreatedAt DATETIME2 NOT NULL
    );

    PRINT 'Employees Table is created';
END

ELSE 

BEGIN
    PRINT 'Employees Table already exists';
END

GO

--Adding values in Employees Table
IF NOT EXISTS(SELECT 1 FROM dbo.Employees)
BEGIN
    INSERT INTO dbo.Employees (FirstName, LastName, Email, Age, DateOfBirth, Nationality, Gender, PhoneNumber, IsCompleted, CreateAt)
    VALUES ('James', 'Anderson', 'JamesAnderson@example.com', '42', '1983-06-07', 'British', 'Male', '1234567891', 1, GETUTCDATE()),
           ('Emily', 'Johnson', 'EmilyJohnson@example.com', '35', '1990-03-29', 'American', 'Female', '1987654321', 1, GETUTCDATE()),
           ('Michael', 'Smith', 'MichaelSmith@example.com', '29', '1996-05-04', 'Scottish', 'Male', '4567891238', 1, GETUTCDATE()),
           ('Sophia', 'Brown', 'SophiaBrown@example.com', '32', '1993-08-15', 'Greek-American', 'Female', '7894561237', 1, GETUTCDATE());

    PRINT 'Employees were added in the table dbo.Employees';
END

ELSE

BEGIN
    PRINT 'dbo.Employees already has values';
END

GO

--Creating Credentials Table
IF OBJECT_ID('dbo.Credentials', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Credentials
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Username NVARCHAR(50) NOT NULL,
        Password NVARCHAR(255) NOT NULL,
        LastLogIn DATETIME2 NOT NULL,
        EmployeeId INT NOT NULL,
        AccountStatus NVARCHAR(20) NOT NULL
    );

    PRINT 'Credentails Table was created';
END

ELSE

BEGIN
    PRINT 'Credentials Table already exists';
END

GO

--Adding Constraint in Credentials Table referencing the Employees Table
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Credentials_Employees' AND parent_object_id = OBJECT_ID('dbo.Credentials'))
BEGIN
    ALTER TABLE Credentials
    ADD CONSTRAINT FK_Credentials_Employees FOREIGN KEY (EmployeeId)
    REFERENCES dbo.Employees(EmployeeId);

    PRINT 'FK_Credentials_Employees created';
END

ELSE

BEGIN
    PRINT 'FK_Credentials_Employees already exists';
END

GO


--Adding values in the Credentials Table
--The password for James Anderson is Anderson123!
--The password for Emily Johnson is Johnson123!
--The password for Michael Smith is Smith123!
--The password for Sophia Brown is Brown123!
--The above are mock passwords that can be used to log in to each account, since it's account will have a different role, to check how the authentication works depending on the role
--You can find an md file on github with the password and roles of each account
IF NOT EXISTS(SELECT 1 FROM dbo.Credentials)
BEGIN
    INSERT INTO Credentials (Username, Password, LastLogIn, EmployeeId, AccountStatus)
    VALUES ('JamesAnderson', 'AQAAAAEAACcQAAAAENZV9APVFFpWhdMaOjZQdpmEtKByVGLvyM141oKVT8HsoSrbESg1cNWtiDIRiFS9/g==', GETUTCDATE(), 1, 'Active'),
           ('EmilyJohnson', 'AQAAAAEAACcQAAAAEGRayAVXh+JaQfXkp0VTHiO4Jyzpdh83Zfc+lBFtKmamg3xfUAZDXgGu8QVU6uX0wQ==', GETUTCDATE(), 2, 'Active'),
           ('MichaelSmith', 'AQAAAAEAACcQAAAAEMxx2yKwXRYKlwKgXqqpOCQXDSvOxuNTxsDdMjcIy4hKBEe8rkgzpml9yU227N49bg==', GETUTCDATE(), 3, 'Active'),
           ('SophiaBrown', 'AQAAAAEAACcQAAAAEGQUe/9Gc9GA2jXRWZ+iNlvYeYnOPskuyiFwUsi1zrGuhB8aJJCjzuwGQyCgnpVyfQ==', GETUTCDATE(), 4, 'Active')

    PRINT 'Values added to Credentails';
END

ELSE

BEGIN
    PRINT 'Values already exists in Credentials Table';
END

--Creating AdditionalDetails Table
IF OBJECT_ID('dbo.AdditionalDetails', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.AdditionalDetails
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        EmergencyNumbers NVARCHAR(max) NOT NULL,
        Education NVARCHAR(100) NOT NULL,
        EmployeeId INT NOT NULL
    );
END

GO

--Adding Constraint in AdditionalDetails Table referencing the Employees Table
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_AdditionalDetails_Employees' AND parent_object_id = OBJECT_ID('dbo.AdditionalDetails'))
BEGIN
    ALTER TABLE AdditionalDetails
    ADD CONSTRAINT FK_AdditionalDetails_Employees FOREIGN KEY (EmployeeId)
    REFERENCES dbo.Employees(EmployeeId);

    PRINT 'FK_AdditionalDetails_Employees created'
END

ELSE

BEGIN 
    PRINT 'FK_AdditionalDetails_Employees already exists';
END

GO

--Adding values in the AdditionalDetails Table
IF NOT EXISTS(SELECT 1 FROM dbo.AdditionalDetails)
BEGIN
    INSERT INTO AdditionalDetails (EmergencyNumbers, Education, EmployeeId)
    VALUES ('+447912345678', 'Data Science', 1),
           ('+447911223344', 'Data Analytics', 2),
           ('+447900112233', 'Computer Science', 3),
           ('+447988776655', 'Accountant', 4);
    
    PRINT 'Values are added on Additional Details Table';
END

ELSE

BEGIN
    PRINT 'AdditionalDetails table already has values';
END

--Creating EmploymentDetails Table
IF OBJECT_ID('dbo.EmploymentDetails', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.EmploymentDetails
    (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Position NVARCHAR(50) NOT NULL,
        Department NVARCHAR(50) NOT NULL,
        EmploymentStatus NVARCHAR(10) NOT NULL,
        HireDate DATE NOT NULL,
        ContractType NVARCHAR(10) NOT NULL,
        WorkLocation NVARCHAR(20) NOT NULL,
        EmployeeId INT NOT NULL
    );

    PRINT 'EmploymentDetails Table created';
END

ELSE

BEGIN
    PRINT 'EmploymentDetails Table already exists';
END

--Adding Contraint to EmploymentDetails that references the Employees Table
IF NOT EXISTS(SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_EmploymentDetails_Employees' AND parent_object_id = OBJECT_ID('dbo.EmploymentDetails'))
BEGIN
    ALTER TABLE EmploymentDetails
    ADD CONSTRAINT FK_EmploymentDetails_Employees FOREIGN KEY (EmployeeId)
    REFERENCES dbo.Employees(EmployeeId);

    PRINT 'FK_EmploymentDetails_Employees created';
END

ELSE

BEGIN
    PRINT 'FK_EmploymentDetails_Employees already exists';
END

--Adding values to EmploymentDetails Table
IF NOT EXISTS(SELECT 1 FROM dbo.EmploymentDetails)
BEGIN
    INSERT INTO EmploymentDetails (Position, Department, EmploymentStatus, HireDate, ContractType, WorkLocation, EmployeeId)
    VALUES ('Machine Learning Engineer', 'Artificial intelligence Division', 'Active', '2024-01-01', 'Full-Time', 'Athens', 1),
           ('Financial Data Analyst', 'Data Analytics Department', 'Active', '2020-05-04', 'Full-Time', 'Athens', 2),
           ('Backend Developer', 'Engineering & Development', 'Active', '2021-09-13', 'Full-Time', 'Athens', 3),
           ('Financial Controller', 'Financial Operations Unit', 'Active', '2023-03-22', 'Full-Time', 'Athnes', 4)

    PRINT 'Values added to EmploymentDetails Table';
END

ELSE

BEGIN
    PRINT 'EmploymentDetails Table already has values';
END