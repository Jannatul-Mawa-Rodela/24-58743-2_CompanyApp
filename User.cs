using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeDetails
{
    public class User
    {
        private string connectionString =
            ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();

                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        public int ValidateLogin(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query =
                    "SELECT UserID FROM Users " +
                    "WHERE Username=@Username AND Password=@Password";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string hashedPassword = HashPassword(password);

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    con.Open();

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                        return Convert.ToInt32(result);

                    return 0;
                }
            }
        }

        public bool UsernameExists(string username)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query =
                    "SELECT COUNT(*) FROM Users WHERE Username=@Username";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    con.Open();

                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool RegisterUser(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query =
                    "INSERT INTO Users (Username, Password) " +
                    "VALUES (@Username, @Password)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    string hashedPassword = HashPassword(password);

                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", hashedPassword);

                    con.Open();

                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}