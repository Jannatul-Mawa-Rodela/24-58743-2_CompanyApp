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