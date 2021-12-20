using MinesweeperApp.Models;
using System;
using System.Data.SqlClient;

namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// This class handles database entries and processing for user objects
    /// </summary>
    public class UserLocalSqlDAO : IUserDAO
    {
        //Connection string to local VS created MySQL database
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinesweeperApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// This method retrieves a user's complete information from the given user name and password.
        /// </summary>
        /// <param name="userName">Unique user name to search for in the database</param>
        /// <param name="password">Associated password to validate user with.</param>
        /// <returns>Boolean value of successful location of user credintials. true if they were found, false otherwise.</returns>
        public bool FindUserByUsernameAndPassword(string userName, string password)
        {
            bool success = false;
            
            string query = "SELECT * FROM users WHERE USERNAME = @username and PASSWORD = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.NChar, 40).Value = userName;
                command.Parameters.Add("@password", System.Data.SqlDbType.NChar, 40).Value = password;

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

        /// <summary>
        /// This metthod finds the user's unique Id from thier chosen userName.
        /// </summary>
        /// <param name="userName">The name that the user chose during registration.</param>
        /// <returns>The found Id that matches the user's unique userName. returns -1 if no entries were found.</returns>
        public int GetIdFromUsername(string userName)
        {
            int id = -1;

            string query = "SELECT * FROM users WHERE USERNAME = @username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = userName;

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

        /// <summary>
        /// This metthod finds the user's unique Id from thier given email.
        /// </summary>
        /// <param name="email">The email that the user chose during registration.</param>
        /// <returns>The found Id that matches the user's unique email. returns -1 if no entries were found.</returns>
        public int GetIdFromEmail(string email)
        {
            int id = -1;

            string query = "SELECT * FROM users WHERE EMAIL = @email";

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

        /// <summary>
        /// This method adds a user to the database.
        /// </summary>
        /// <param name="user">The user object with the information to add.</param>
        /// <returns>Boolean value that represents whether the user was successfully added. true if they were added, false otherwise.</returns>
        public bool Add(User user)
        {
            bool success = false;

            string query = "INSERT INTO users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, EMAIL, SEX, AGE, STATE) VALUES (@username, @password, @firstname, @lastname, @email, @sex, @age, @state)";

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
