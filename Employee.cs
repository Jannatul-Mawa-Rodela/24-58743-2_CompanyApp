using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeDetails
{
    class Employee
    {
        private static string myConn =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public int Age { get; set; }
        public string ContactNo { get; set; }
        public string Gender { get; set; }

        private const string SelectQuery =
            "SELECT * FROM Emp_details";

        private const string InsertQuery =
            "INSERT INTO Emp_details " +
            "(EmpId, EmpName, EmpAge, EmpContact, EmpGender) " +
            "VALUES (@EmpId, @EmpName, @EmpAge, @EmpContact, @EmpGender)";

        private const string UpdateQuery =
            "UPDATE Emp_details SET " +
            "EmpName = @EmpName, " +
            "EmpAge = @EmpAge, " +
            "EmpContact = @EmpContact, " +
            "EmpGender = @EmpGender " +
            "WHERE EmpId = @EmpId";

        private const string DeleteQuery =
            "DELETE FROM Emp_details WHERE EmpId = @EmpId";

        // Gets all employees from the database.
        public DataTable GetEmployees()
        {
            DataTable datatable = new DataTable();

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(SelectQuery, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(com))
                    {
                        adapter.Fill(datatable);
                    }
                }
            }

            return datatable;
        }

        // Inserts a new employee into the database.
        public bool InsertEmployee(Employee employee)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(InsertQuery, con))
                {
                    com.Parameters.AddWithValue("@EmpId", employee.EmpId);
                    com.Parameters.AddWithValue("@EmpName", employee.EmpName);
                    com.Parameters.AddWithValue("@EmpAge", employee.Age);
                    com.Parameters.AddWithValue("@EmpContact", employee.ContactNo);
                    com.Parameters.AddWithValue("@EmpGender", employee.Gender);

                    rows = com.ExecuteNonQuery();
                }
            }

            return rows > 0;
        }

        // Updates an existing employee.
        public bool UpdateEmployee(Employee employee)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(UpdateQuery, con))
                {
                    com.Parameters.AddWithValue("@EmpName", employee.EmpName);
                    com.Parameters.AddWithValue("@EmpAge", employee.Age);
                    com.Parameters.AddWithValue("@EmpContact", employee.ContactNo);
                    com.Parameters.AddWithValue("@EmpGender", employee.Gender);
                    com.Parameters.AddWithValue("@EmpId", employee.EmpId);

                    rows = com.ExecuteNonQuery();
                }
            }

            return rows > 0;
        }

        // Deletes an employee using the employee ID.
        public bool DeleteEmployee(Employee employee)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();

                using (SqlCommand com = new SqlCommand(DeleteQuery, con))
                {
                    com.Parameters.AddWithValue("@EmpId", employee.EmpId);

                    rows = com.ExecuteNonQuery();
                }
            }

            return rows > 0;
        }
    }
}