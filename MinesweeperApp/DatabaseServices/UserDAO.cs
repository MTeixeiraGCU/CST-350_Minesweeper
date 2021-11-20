using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.DatabaseServices
{
    public class UserDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinesweeperApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool FindUserByUsernameAndPassword(User userLogin)
        {
            bool success = false;
            
            string query = "SELECT * FROM users WHERE USERNAME = @username and PASSWORD = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@USERNAME", System.Data.SqlDbType.NChar, 40).Value = userLogin.Username;
                command.Parameters.Add("@PASSWORD", System.Data.SqlDbType.NChar, 40).Value = userLogin.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if(reader.HasRows)
                    {
                        success = true;
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }

            return success;
        }

        public int GetIdFromUsername(string username)
        {
            int id = -1;

            string query = "SELECT * FROM dbo.users WHERE USERNAME = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = username;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        id = Convert.ToInt32(reader.GetValue(0));
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return id;
        }

        public int GetIdFromEmail(string email)
        {
            int id = -1;

            string query = "SELECT * FROM dbo.users WHERE EMAIL = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@email", System.Data.SqlDbType.VarChar, 40).Value = email;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        id = Convert.ToInt32(reader.GetValue(0));
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return id;
        }

        public bool Add(User user)
        {
            bool success = false;

            string query = "INSERT INTO dbo.users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, EMAIL, SEX, AGE, STATE) VALUES (@username, @password, @firstname, @lastname, @email, @sex, @age, @state)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 40).Value = user.Username;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 40).Value = user.Password;
                command.Parameters.Add("@firstname", System.Data.SqlDbType.VarChar, 40).Value = user.FirstName;
                command.Parameters.Add("@lastname", System.Data.SqlDbType.NVarChar, 40).Value = user.LastName;
                command.Parameters.Add("@email", System.Data.SqlDbType.NVarChar, 40).Value = user.Email;
                command.Parameters.Add("@sex", System.Data.SqlDbType.NVarChar, 10).Value = user.Sex;
                command.Parameters.Add("@age", System.Data.SqlDbType.Int).Value = user.Age;
                command.Parameters.Add("@state", System.Data.SqlDbType.NVarChar, 10).Value = user.State;

                try
                {
                    connection.Open();
                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        success = true;
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }
    }
}
