# 24-58743-2_CompanyApp
## Merging Login/Register and Employee CRUD into One Application

### Project Overview

This project is the result of merging two separate Windows Forms applications into one complete application.

Originally, there were two applications:

* **Login-and-Register** — used MS Access (`db_users.mdb`) with `System.Data.OleD`,which was converted to SQL Server LocalDB using `System.Data.SqlClient`.
* **EmployeeDetails** — used SQL Server LocalDB with `System.Data.SqlClient`.

The final application uses:

* **One application**
* **One Visual Studio project**
* **One SQL Server LocalDB database**
* **One login system**
* **Employee CRUD after successful login**
* **Logout and return to the login screen**

The final database is named **`dbCompanyApp`**.

---

## Technologies Used

* C#
* Windows Forms
* .NET Framework 4.8
* SQL Server LocalDB
* ADO.NET
* `System.Data.SqlClient`
* Visual Studio
* GitHub

---

# 1. Before and After

### Before

There were two separate applications:

```text
Login-and-Register
        |
        └── MS Access
             └── db_users.mdb
                └── SQL Server LocalDB
                    └── db_users

EmployeeDetails
        |
        └── SQL Server LocalDB
             └── Employee Database
```

This created separate login and employee systems.

### After

The two applications were merged into one:

```text
                 CompanyApp
                     |
                 frmLogin
                     |
              Successful Login
                     |
                frmDashboard
                     |
              Manage Employees
                     |
                frmEmployee
                     |
                dbCompanyApp
                 /          \
             Users       Emp_details
```

The application now uses a single database and a single SQL Server provider.

---

# 2. The Six Conflicts and Their Solutions

## Conflict 1: Different Namespaces

The original projects used different namespaces:

```text
Login_and_Register
EmployeeDetails
```

### Solution

The imported login/register forms were changed to the host project's namespace:

```csharp
namespace EmployeeDetails
```

The namespace was fixed in both the `.cs` and `.Designer.cs` files for:

* `frmLogin`
* `frmRegister`
* `frmDashboard`

This made the partial form classes compile correctly.

---

## Conflict 2: Different Data Providers

The Login/Register application used:

```csharp
System.Data.OleDb
```

because it connected to an Access `.mdb` file.

The EmployeeDetails application used:

```csharp
System.Data.SqlClient
```

because it connected to SQL Server.

### Solution

The final application completely removed `OleDb` and uses:

```csharp
System.Data.SqlClient
```

for both authentication and employee operations.

The Access connection was replaced with the SQL Server LocalDB connection.

SQL parameters were also changed from positional parameters to named parameters such as:

```text
@Username
@Password
@EmpId
@EmpName
```

This also prevents SQL injection caused by directly concatenating user input into SQL queries.

---

# 3. Unified Database

A new SQL Server LocalDB database was created:

```text
dbCompanyApp
```

It contains two tables:

```text
dbo.Users
dbo.Emp_details
```

### Users Table

```text
UserID       INT IDENTITY PRIMARY KEY
Username     NVARCHAR(50) UNIQUE NOT NULL
Password     NVARCHAR(200) NOT NULL
CreatedAt    DATETIME DEFAULT GETDATE()
```

### Emp_details Table

```text
EmpId        NVARCHAR(50) PRIMARY KEY
EmpName      NVARCHAR(100) NOT NULL
EmpAge       INT NOT NULL
EmpContact   NVARCHAR(20)
EmpGender    NVARCHAR(10)
CreatedBy    INT NULL
```

The `CreatedBy` field is connected to:

```text
Users.UserID
```

through a foreign key.

The `CreatedBy` field is nullable because employees migrated from the old system did not have a known creator.

---

# 4. Access Data Migration

The original login accounts were stored in:

```text
db_users.mdb
```

The account information was migrated into:

```text
dbo.Users
```

Only `Username` and `Password` were inserted.

`UserID` was not manually inserted because SQL Server generates it automatically using `IDENTITY`.

The final project does not depend on the old `.mdb` file.

---

# 5. Schema.sql

The database structure was documented in `Schema.sql`.

The main database design is:

```sql
CREATE DATABASE dbCompanyApp;
```

The `Users` table stores login information, while `Emp_details` stores employee information.

The relationship is:

```text
Users.UserID
      |
      | 1
      |
      | many
      |
Emp_details.CreatedBy
```

This allows each employee record to be associated with the user who created it.

---

# 6. Choosing the Host Project

The **EmployeeDetails** project was selected as the host project.

This was done because:

* It already used SQL Server LocalDB.
* It already had an `App.config`.
* It already used `System.Data.SqlClient`.
* It already contained the employee data-access class.
* It targeted .NET Framework 4.8.

The Login/Register forms were imported into this project instead of creating a third project.

The final application contains one executable and one startup point.

---

# 7. Importing the Forms

Three forms from the Login/Register application were imported:

* `frmLogin`
* `frmRegister`
* `frmDashboard`

Each Windows Form contains three related files:

```text
frmLogin.cs
frmLogin.Designer.cs
frmLogin.resx
```

The same structure was maintained for the other two forms.

Therefore, a total of nine form files were imported.

The `.cs` files contain the form logic, `.Designer.cs` contains the generated controls and layout, and `.resx` contains form resources.

`Program.cs`, `App.config`, `Properties`, and the second `.csproj` were not imported.

---

# 8. User.cs

A new `User.cs` data-access class was created to handle authentication and registration.

It provides methods for:

### Login

```text
ValidateLogin(username, password)
```

This returns the logged-in user's `UserID` when the credentials are correct.

### Username Check

```text
UsernameExists(username)
```

This checks whether a username already exists.

### Registration

```text
RegisterUser(username, password)
```

This creates a new user in the `Users` table.

The login process returns the `UserID` because that ID is required for the `CreatedBy` field in the employee table.

---

# 9. Session Management

A static `Session` class was added to keep the currently logged-in user's information.

It stores:

```text
UserID
Username
```

After successful login:

```text
Session.UserID = logged-in user's ID
Session.Username = logged-in username
```

The Employee CRUD form then uses:

```text
Session.UserID
```

to set the `CreatedBy` value when a new employee is added.

The session is cleared during logout.

---

# 10. One Connection String

Only one database connection is used by the application.

The connection string is stored in `App.config` and points to:

```text
dbCompanyApp
```

The application uses `ConfigurationManager` to retrieve the connection string.

No hard-coded Access connection string remains in the project.

The old:

```text
db_users.mdb
```

dependency and `OleDb` provider were completely removed.

---

# 11. Application Flow

The final application starts with:

```text
frmLogin
```

The user can either:

* Login with an existing account
* Open the registration form and create a new account

After successful login:

```text
frmLogin
    ↓
frmDashboard
    ↓
Manage Employees
    ↓
frmEmployee
```

The employee form provides CRUD operations.

---

# 12. Logout

The logout function does not immediately terminate the entire application.

The process is:

```text
Logout
   ↓
Confirmation
   ↓
Session.Clear()
   ↓
Create a new frmLogin
   ↓
Show Login
   ↓
Close Dashboard
```

A new login form is created because the original login form was hidden after successful login.

This avoids the hidden-form problem where the application could remain running without a visible window.

The login form also handles `FormClosed` so that the application exits correctly when the user closes the login window.

---

# 13. CreatedBy Relationship

When a logged-in user adds an employee, the application sets:

```csharp
employee.CreatedBy = Session.UserID;
```

The employee data is therefore connected to the logged-in user.

The employee list uses a `LEFT JOIN`:

```sql
SELECT
    e.EmpId,
    e.EmpName,
    e.EmpAge,
    e.EmpContact,
    e.EmpGender,
    u.Username AS CreatedBy
FROM Emp_details e
LEFT JOIN Users u
    ON e.CreatedBy = u.UserID;
```

### Why LEFT JOIN?

`LEFT JOIN` is used instead of an inner `JOIN` because some employee records may have:

```text
CreatedBy = NULL
```

These records may have come from the old system before the `CreatedBy` relationship existed.

With `LEFT JOIN`, those employees are still displayed in the grid.

---

# 14. Handling the Schema Change

The original employee table did not contain `CreatedBy`.

After adding the new field, the Employee model and INSERT operation were updated to include it.

The employee grid was also updated to display the creator.

The grid is bound using column names rather than depending on a fixed column position.

The creator is displayed using:

```text
CreatedBy
```

which comes from:

```sql
u.Username AS CreatedBy
```

---

# 15. One Project and One Entry Point

The final solution contains only one application project.

The second `Program.cs` was not imported.

The application uses one startup entry point:

```csharp
Application.Run(new frmLogin());
```

This prevents the `CS0017` error caused by having multiple `Main()` methods.

---

# 16. Framework Version

The final host project targets:

```text
.NET Framework 4.8
```

The original Login/Register project used .NET Framework 4.7.2.

The imported forms were adapted to the host project's .NET Framework 4.8 environment.

---

# 17. Build Error and Fix

During the merge, one possible issue was caused by the imported forms still using the original namespace:

```text
Login_and_Register
```

while the host project used:

```text
EmployeeDetails
```

This caused partial class and control-reference compilation errors.

The problem was fixed by changing the namespace in all imported form files, including both the main `.cs` files and `.Designer.cs` files.

After rebuilding the solution, the forms compiled correctly.

Another important fix was replacing the old Access/OleDb database code with SQL Server/SqlClient code.

---

# 18. Why One Database Is Better Than Two

Using one database makes the application easier to manage because all related information is stored in one place. The login users and employee records can be connected directly using the `UserID` and `CreatedBy` relationship. This avoids keeping two separate databases synchronized. For example, a `LEFT JOIN` can display the username of the person who created an employee record while still showing employees whose creator is unknown. A single database also makes backup, maintenance, querying, and future development easier.

---

# 19. Project Structure

The main structure of the final application is:

```text
<YourID>_CompanyApp
│
├── App.config
├── Program.cs
├── Schema.sql
├── README.md
│
├── User.cs
├── Session.cs
├── Employee.cs
│
├── frmLogin.cs
├── frmLogin.Designer.cs
├── frmLogin.resx
│
├── frmRegister.cs
├── frmRegister.Designer.cs
├── frmRegister.resx
│
├── frmDashboard.cs
├── frmDashboard.Designer.cs
├── frmDashboard.resx
│
└── frmEmployee.cs
```

---

# 20. Final Testing

The following application flow was tested:

* [x] Application starts at Login
* [x] Existing user can login
* [x] New user can register
* [x] Dashboard opens after login
* [x] Employee management opens from Dashboard
* [x] Employee can be added
* [x] Employee can be updated
* [x] Employee can be deleted
* [x] Employee records show the creator
* [x] Logout returns to a fresh Login screen
* [x] Session information is cleared during logout
* [x] Application exits correctly
* [x] No MS Access `.mdb` dependency remains
* [x] No `System.Data.OleDb` code remains
* [x] One SQL Server database is used
* [x] One project and one executable are used

---
# 21. Screenshots:
Database tables:
<img width="1315" height="742" alt="image" src="https://github.com/user-attachments/assets/0c119012-f8e6-4566-be31-36cfdb8fd08d" />  
<img width="1085" height="737" alt="image" src="https://github.com/user-attachments/assets/e5d7791c-ceb1-453d-8235-496707f9f9d1" />

Solution Explorer:
<img width="592" height="1020" alt="image" src="https://github.com/user-attachments/assets/bf016758-7d0e-41e9-bc02-c88a123e5a9b" />

Working Application:
[Log in page]
<img width="417" height="657" alt="image" src="https://github.com/user-attachments/assets/71d6d315-d48b-4816-a434-948c77cd0e86" />
[Register page]
<img width="367" height="716" alt="image" src="https://github.com/user-attachments/assets/a94065a5-5f4d-434b-9892-b0769fa1fa58" />

[DashBoard]
<img width="1007" height="742" alt="image" src="https://github.com/user-attachments/assets/2ed76869-31a0-4f81-b7c4-ca4fab65e22a" />
[Employee management-Employee Details]
<img width="1175" height="640" alt="image" src="https://github.com/user-attachments/assets/1e572b92-be1f-4393-8d45-05b2dabf89c5" />

# 22.Bonus Features

The bonus features were not implemented in this version of the project. The main required features of Lab 2 were completed, including the unified database, login and registration, employee CRUD operations, session management, `CreatedBy` relationship, and logout functionality.

Possible bonus features such as SHA-256 password hashing, role-based access control, login history, and search-by-name with delete confirmation were considered as future improvements.


# 23. Submission Fils

The final submission contains:

```text
GitHub Repository
│
├── Source Code
├── README.md
├── Schema.sql
├── .gitignore
└── Report.pdf
```

The `bin`, `obj`, and `.vs` folders are excluded from the GitHub repository.

The old `db_users.mdb` file is also not included.

---

## 24. Conclusion

The Login/Register and Employee CRUD applications were successfully merged into one Windows Forms application. The final system uses SQL Server LocalDB as the single database and `System.Data.SqlClient` as the single database provider. Login and registration are connected to the `Users` table, while employee CRUD operations use the `Emp_details` table. The `Session` class connects the logged-in user with employee records through the `CreatedBy` foreign key. The final application therefore provides one integrated login, dashboard, employee management, and logout workflow.
