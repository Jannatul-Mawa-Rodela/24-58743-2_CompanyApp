CREATE DATABASE dbEmployeeDetails;
GO
USE dbEmployeeDetails;
GO

CREATE TABLE Emp_details
(
    EmpId NVARCHAR(50) PRIMARY KEY,
    EmpName NVARCHAR(100) NOT NULL,
    EmpAge INT NOT NULL,
    EmpContact NVARCHAR(20),
    EmpGender NVARCHAR(10)
);
GO
SELECT * FROM dbo.Emp_details;
INSERT INTO dbo.Emp_details
    (EmpId, EmpName, EmpAge, EmpContact, EmpGender)
VALUES
    ('E001', 'Test Employee', 22, '01700000000', 'Male');