-- =============================================
-- Team Tasks Dashboard - Database Setup Script
-- SQL Server Version
-- =============================================

-- Drop database if exists and create new
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'TeamTasksSample')
BEGIN
    ALTER DATABASE TeamTasksSample SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TeamTasksSample;
END
GO

CREATE DATABASE TeamTasksSample;
GO

USE TeamTasksSample;
GO

-- =============================================
-- Create Tables
-- =============================================

-- Developers Table
CREATE TABLE Developers (
    DeveloperId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Projects Table
CREATE TABLE Projects (
    ProjectId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    ClientName NVARCHAR(200) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('Planned', 'InProgress', 'Completed')),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);

-- Tasks Table
CREATE TABLE Tasks (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT NOT NULL,
    Title NVARCHAR(300) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    AssigneeId INT NOT NULL,
    Status NVARCHAR(50) NOT NULL CHECK (Status IN ('ToDo', 'InProgress', 'Blocked', 'Completed')),
    Priority NVARCHAR(50) NOT NULL CHECK (Priority IN ('Low', 'Medium', 'High')),
    EstimatedComplexity INT NOT NULL CHECK (EstimatedComplexity BETWEEN 1 AND 5),
    DueDate DATE NOT NULL,
    CompletionDate DATE NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Tasks_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
    CONSTRAINT FK_Tasks_Developers FOREIGN KEY (AssigneeId) REFERENCES Developers(DeveloperId)
);

-- Create Indexes for better performance
CREATE INDEX IX_Tasks_ProjectId ON Tasks(ProjectId);
CREATE INDEX IX_Tasks_AssigneeId ON Tasks(AssigneeId);
CREATE INDEX IX_Tasks_Status ON Tasks(Status);
CREATE INDEX IX_Tasks_DueDate ON Tasks(DueDate);

GO

-- =============================================
-- Insert Sample Data
-- =============================================

-- Insert 5 Active Developers
INSERT INTO Developers (FirstName, LastName, Email, IsActive, CreatedAt) VALUES
('Carlos', 'Mendoza', 'carlos.mendoza@teamtasks.com', 1, DATEADD(DAY, -90, GETDATE())),
('Ana', 'Rodriguez', 'ana.rodriguez@teamtasks.com', 1, DATEADD(DAY, -85, GETDATE())),
('Luis', 'Fernandez', 'luis.fernandez@teamtasks.com', 1, DATEADD(DAY, -80, GETDATE())),
('Maria', 'Garcia', 'maria.garcia@teamtasks.com', 1, DATEADD(DAY, -75, GETDATE())),
('Pedro', 'Martinez', 'pedro.martinez@teamtasks.com', 1, DATEADD(DAY, -70, GETDATE()));

-- Insert 3 Projects
INSERT INTO Projects (Name, ClientName, StartDate, EndDate, Status, CreatedAt) VALUES
('E-Commerce Platform', 'TechRetail Corp', DATEADD(DAY, -60, GETDATE()), DATEADD(DAY, 30, GETDATE()), 'InProgress', DATEADD(DAY, -60, GETDATE())),
('Mobile Banking App', 'FinanceBank SA', DATEADD(DAY, -45, GETDATE()), DATEADD(DAY, 45, GETDATE()), 'InProgress', DATEADD(DAY, -45, GETDATE())),
('CRM System', 'SalesForce Ltd', DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, 60, GETDATE()), 'Planned', DATEADD(DAY, -30, GETDATE()));

-- Insert 20+ Tasks with varied statuses and dates
INSERT INTO Tasks (ProjectId, Title, Description, AssigneeId, Status, Priority, EstimatedComplexity, DueDate, CompletionDate, CreatedAt) VALUES
-- E-Commerce Platform (ProjectId = 1)
(1, 'Design database schema', 'Create normalized database schema for products and orders', 1, 'Completed', 'High', 4, DATEADD(DAY, -50, GETDATE()), DATEADD(DAY, -48, GETDATE()), DATEADD(DAY, -55, GETDATE())),
(1, 'Implement product catalog API', 'REST API for product CRUD operations', 2, 'Completed', 'High', 5, DATEADD(DAY, -40, GETDATE()), DATEADD(DAY, -35, GETDATE()), DATEADD(DAY, -45, GETDATE())),
(1, 'Create shopping cart functionality', 'Add to cart, update quantities, remove items', 3, 'InProgress', 'High', 4, DATEADD(DAY, 5, GETDATE()), NULL, DATEADD(DAY, -30, GETDATE())),
(1, 'Payment gateway integration', 'Integrate Stripe payment processing', 1, 'InProgress', 'High', 5, DATEADD(DAY, 10, GETDATE()), NULL, DATEADD(DAY, -25, GETDATE())),
(1, 'Order management system', 'Backend for order processing and tracking', 4, 'ToDo', 'Medium', 4, DATEADD(DAY, 15, GETDATE()), NULL, DATEADD(DAY, -20, GETDATE())),
(1, 'Email notification service', 'Send order confirmations and updates', 5, 'ToDo', 'Medium', 3, DATEADD(DAY, 20, GETDATE()), NULL, DATEADD(DAY, -15, GETDATE())),
(1, 'Product search and filters', 'Implement search with filters by category, price', 2, 'Blocked', 'Medium', 3, DATEADD(DAY, 3, GETDATE()), NULL, DATEADD(DAY, -10, GETDATE())),

-- Mobile Banking App (ProjectId = 2)
(2, 'User authentication module', 'Implement secure login with 2FA', 1, 'Completed', 'High', 5, DATEADD(DAY, -35, GETDATE()), DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -40, GETDATE())),
(2, 'Account balance API', 'Real-time account balance retrieval', 3, 'Completed', 'High', 4, DATEADD(DAY, -25, GETDATE()), DATEADD(DAY, -26, GETDATE()), DATEADD(DAY, -30, GETDATE())),
(2, 'Money transfer functionality', 'Internal and external transfers', 2, 'InProgress', 'High', 5, DATEADD(DAY, 8, GETDATE()), NULL, DATEADD(DAY, -20, GETDATE())),
(2, 'Transaction history', 'Display and filter transaction history', 4, 'InProgress', 'Medium', 3, DATEADD(DAY, 12, GETDATE()), NULL, DATEADD(DAY, -15, GETDATE())),
(2, 'Bill payment integration', 'Pay utility bills through the app', 5, 'ToDo', 'Medium', 4, DATEADD(DAY, 18, GETDATE()), NULL, DATEADD(DAY, -10, GETDATE())),
(2, 'Push notifications', 'Real-time alerts for transactions', 1, 'ToDo', 'Low', 2, DATEADD(DAY, 25, GETDATE()), NULL, DATEADD(DAY, -5, GETDATE())),
(2, 'Biometric authentication', 'Fingerprint and face recognition', 3, 'Blocked', 'High', 4, DATEADD(DAY, 6, GETDATE()), NULL, DATEADD(DAY, -8, GETDATE())),

-- CRM System (ProjectId = 3)
(3, 'Contact management module', 'CRUD operations for customer contacts', 2, 'Completed', 'High', 4, DATEADD(DAY, -20, GETDATE()), DATEADD(DAY, -18, GETDATE()), DATEADD(DAY, -25, GETDATE())),
(3, 'Sales pipeline dashboard', 'Visual representation of sales stages', 4, 'InProgress', 'High', 5, DATEADD(DAY, 7, GETDATE()), NULL, DATEADD(DAY, -15, GETDATE())),
(3, 'Lead scoring system', 'Automatic lead qualification and scoring', 5, 'InProgress', 'Medium', 4, DATEADD(DAY, 14, GETDATE()), NULL, DATEADD(DAY, -10, GETDATE())),
(3, 'Email campaign integration', 'Integrate with email marketing tools', 1, 'ToDo', 'Medium', 3, DATEADD(DAY, 22, GETDATE()), NULL, DATEADD(DAY, -8, GETDATE())),
(3, 'Reporting and analytics', 'Sales reports and KPI dashboards', 3, 'ToDo', 'High', 5, DATEADD(DAY, 28, GETDATE()), NULL, DATEADD(DAY, -5, GETDATE())),
(3, 'Mobile app sync', 'Sync CRM data with mobile application', 2, 'ToDo', 'Low', 3, DATEADD(DAY, 35, GETDATE()), NULL, DATEADD(DAY, -3, GETDATE())),
(3, 'Customer support ticketing', 'Integrated support ticket system', 4, 'Blocked', 'Medium', 4, DATEADD(DAY, 4, GETDATE()), NULL, DATEADD(DAY, -7, GETDATE()));

GO

-- =============================================
-- 2.2.1 Resumen de carga por desarrollador
-- =============================================
CREATE OR ALTER VIEW vw_DeveloperWorkload AS
SELECT 
    CONCAT(d.FirstName, ' ', d.LastName) AS DeveloperName,
    d.DeveloperId,
    COUNT(t.TaskId) AS OpenTasksCount,
    ISNULL(AVG(CAST(t.EstimatedComplexity AS DECIMAL(10,2))), 0) AS AverageEstimatedComplexity
FROM Developers d
LEFT JOIN Tasks t ON d.DeveloperId = t.AssigneeId AND t.Status <> 'Completed'
WHERE d.IsActive = 1
GROUP BY d.DeveloperId, d.FirstName, d.LastName;
GO

-- Query to test
-- SELECT * FROM vw_DeveloperWorkload ORDER BY OpenTasksCount DESC;

-- =============================================
-- 2.2.2 Resumen de estado por proyecto
-- =============================================
CREATE OR ALTER VIEW vw_ProjectHealth AS
SELECT 
    p.ProjectId,
    p.Name AS ProjectName,
    p.ClientName,
    p.Status AS ProjectStatus,
    COUNT(t.TaskId) AS TotalTasks,
    SUM(CASE WHEN t.Status <> 'Completed' THEN 1 ELSE 0 END) AS OpenTasks,
    SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
FROM Projects p
LEFT JOIN Tasks t ON p.ProjectId = t.ProjectId
GROUP BY p.ProjectId, p.Name, p.ClientName, p.Status;
GO

-- Query to test
-- SELECT * FROM vw_ProjectHealth;

-- =============================================
-- 2.2.3 Tareas próximas a vencer
-- =============================================
CREATE OR ALTER VIEW vw_TasksDueSoon AS
SELECT 
    t.TaskId,
    t.Title,
    t.DueDate,
    t.Priority,
    t.Status,
    CONCAT(d.FirstName, ' ', d.LastName) AS AssigneeName,
    p.Name AS ProjectName,
    DATEDIFF(DAY, GETDATE(), t.DueDate) AS DaysUntilDue
FROM Tasks t
INNER JOIN Developers d ON t.AssigneeId = d.DeveloperId
INNER JOIN Projects p ON t.ProjectId = p.ProjectId
WHERE t.DueDate BETWEEN CAST(GETDATE() AS DATE) AND DATEADD(DAY, 7, CAST(GETDATE() AS DATE))
  AND t.Status <> 'Completed';
GO

-- Query to test
-- SELECT * FROM vw_TasksDueSoon ORDER BY DueDate;

-- =============================================
-- 2.2.4 Stored Procedure: Insertar nueva tarea
-- =============================================
CREATE OR ALTER PROCEDURE sp_InsertTask
    @ProjectId INT,
    @Title NVARCHAR(300),
    @Description NVARCHAR(MAX) = NULL,
    @AssigneeId INT,
    @Status NVARCHAR(50),
    @Priority NVARCHAR(50),
    @EstimatedComplexity INT,
    @DueDate DATE,
    @CompletionDate DATE = NULL,
    @TaskId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Validate ProjectId exists
    IF NOT EXISTS (SELECT 1 FROM Projects WHERE ProjectId = @ProjectId)
    BEGIN
        RAISERROR('El ProjectId especificado no existe.', 16, 1);
        RETURN;
    END
    
    -- Validate AssigneeId exists and is active
    IF NOT EXISTS (SELECT 1 FROM Developers WHERE DeveloperId = @AssigneeId AND IsActive = 1)
    BEGIN
        RAISERROR('El AssigneeId especificado no existe o el desarrollador no está activo.', 16, 1);
        RETURN;
    END
    
    -- Validate Status
    IF @Status NOT IN ('ToDo', 'InProgress', 'Blocked', 'Completed')
    BEGIN
        RAISERROR('Status debe ser: ToDo, InProgress, Blocked, o Completed.', 16, 1);
        RETURN;
    END
    
    -- Validate Priority
    IF @Priority NOT IN ('Low', 'Medium', 'High')
    BEGIN
        RAISERROR('Priority debe ser: Low, Medium, o High.', 16, 1);
        RETURN;
    END
    
    -- Validate EstimatedComplexity
    IF @EstimatedComplexity < 1 OR @EstimatedComplexity > 5
    BEGIN
        RAISERROR('EstimatedComplexity debe estar entre 1 y 5.', 16, 1);
        RETURN;
    END
    
    -- Validate Title is not empty
    IF @Title IS NULL OR LTRIM(RTRIM(@Title)) = ''
    BEGIN
        RAISERROR('El título de la tarea es requerido.', 16, 1);
        RETURN;
    END
    
    -- Insert the task
    BEGIN TRY
        INSERT INTO Tasks (ProjectId, Title, Description, AssigneeId, Status, Priority, EstimatedComplexity, DueDate, CompletionDate, CreatedAt)
        VALUES (@ProjectId, @Title, @Description, @AssigneeId, @Status, @Priority, @EstimatedComplexity, @DueDate, @CompletionDate, GETDATE());
        
        SET @TaskId = SCOPE_IDENTITY();
        
        SELECT 'Tarea creada exitosamente.' AS Message, @TaskId AS TaskId;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Test the stored procedure
-- DECLARE @NewTaskId INT;
-- EXEC sp_InsertTask 
--     @ProjectId = 1,
--     @Title = 'Test Task',
--     @Description = 'This is a test task',
--     @AssigneeId = 1,
--     @Status = 'ToDo',
--     @Priority = 'Medium',
--     @EstimatedComplexity = 3,
--     @DueDate = '2026-02-15',
--     @TaskId = @NewTaskId OUTPUT;
-- SELECT @NewTaskId AS CreatedTaskId;

-- =============================================
-- 2.3 Developer Delay Risk Prediction
-- =============================================
CREATE OR ALTER VIEW vw_DeveloperDelayRisk AS
WITH CompletedTasksStats AS (
    -- Calculate average delay for completed tasks per developer
    SELECT 
        AssigneeId,
        AVG(CASE 
            WHEN CompletionDate > DueDate THEN DATEDIFF(DAY, DueDate, CompletionDate)
            ELSE 0 
        END) AS AvgDelayDays
    FROM Tasks
    WHERE Status = 'Completed' AND CompletionDate IS NOT NULL
    GROUP BY AssigneeId
),
OpenTasksStats AS (
    -- Get open tasks statistics per developer
    SELECT 
        AssigneeId,
        COUNT(*) AS OpenTasksCount,
        MIN(DueDate) AS NearestDueDate,
        MAX(DueDate) AS LatestDueDate
    FROM Tasks
    WHERE Status <> 'Completed'
    GROUP BY AssigneeId
)
SELECT 
    d.DeveloperId,
    CONCAT(d.FirstName, ' ', d.LastName) AS DeveloperName,
    ISNULL(ots.OpenTasksCount, 0) AS OpenTasksCount,
    ISNULL(cts.AvgDelayDays, 0) AS AvgDelayDays,
    ots.NearestDueDate,
    ots.LatestDueDate,
    DATEADD(DAY, ISNULL(cts.AvgDelayDays, 0), ots.LatestDueDate) AS PredictedCompletionDate,
    CASE 
        WHEN DATEADD(DAY, ISNULL(cts.AvgDelayDays, 0), ots.LatestDueDate) > ots.LatestDueDate 
             OR ISNULL(cts.AvgDelayDays, 0) > 3 
        THEN 1 
        ELSE 0 
    END AS HighRiskFlag
FROM Developers d
LEFT JOIN CompletedTasksStats cts ON d.DeveloperId = cts.AssigneeId
LEFT JOIN OpenTasksStats ots ON d.DeveloperId = ots.AssigneeId
WHERE d.IsActive = 1 AND ots.OpenTasksCount > 0;
GO

-- Query to test
-- SELECT * FROM vw_DeveloperDelayRisk ORDER BY HighRiskFlag DESC, AvgDelayDays DESC;

-- =============================================
-- Additional useful queries
-- =============================================

-- Get all developers (for dropdowns)
CREATE OR ALTER VIEW vw_ActiveDevelopers AS
SELECT 
    DeveloperId,
    CONCAT(FirstName, ' ', LastName) AS FullName,
    Email,
    FirstName,
    LastName
FROM Developers
WHERE IsActive = 1;
GO

-- Get all projects with task counts
CREATE OR ALTER VIEW vw_ProjectsWithStats AS
SELECT 
    p.ProjectId,
    p.Name,
    p.ClientName,
    p.StartDate,
    p.EndDate,
    p.Status,
    COUNT(t.TaskId) AS TotalTasks,
    SUM(CASE WHEN t.Status <> 'Completed' THEN 1 ELSE 0 END) AS OpenTasks,
    SUM(CASE WHEN t.Status = 'Completed' THEN 1 ELSE 0 END) AS CompletedTasks
FROM Projects p
LEFT JOIN Tasks t ON p.ProjectId = t.ProjectId
GROUP BY p.ProjectId, p.Name, p.ClientName, p.StartDate, p.EndDate, p.Status;
GO

PRINT 'Database setup completed successfully!';
PRINT 'Database: TeamTasksSample';
PRINT 'Tables created: Developers, Projects, Tasks';
PRINT 'Sample data inserted: 5 developers, 3 projects, 20+ tasks';
PRINT 'Views created: vw_DeveloperWorkload, vw_ProjectHealth, vw_TasksDueSoon, vw_DeveloperDelayRisk';
PRINT 'Stored procedure created: sp_InsertTask';
GO
