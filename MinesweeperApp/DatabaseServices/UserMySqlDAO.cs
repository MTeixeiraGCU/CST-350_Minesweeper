using MinesweeperApp.Models;
using MySqlConnector;
using System;

namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// Implementation of the IUserDAO using a MySql database structure.
    /// </summary>
    public class UserMySqlDAO : IUserDAO
    {
        private static string database_server = Environment.GetEnvironmentVariable("DATABASE_SERVER_NAME");
        private static string database_userId = Environment.GetEnvironmentVariable("DATABASE_USER_ID");
        private static string database_password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        private static string database_schema = Environment.GetEnvironmentVariable("DATABASE_SCHEMA");
        private static string database_port = Environment.GetEnvironmentVariable("DATABASE_PORT");

        //MySQL database connection string.
        private static string connectionString = "server=" + database_server + ";UserId=" + database_userId + ";password=" + database_password + ";database=" + database_schema + ";port=" + database_port;

        public bool FindUserByUsernameAndPassword(string userName, string password)
        {
            bool success = false;

            string query = "SELECT * FROM users WHERE USERNAME = @username and PASSWORD = @password";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@username", userName));
                command.Parameters.Add(new MySqlParameter("@password", password));

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
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

        public int GetIdFromUsername(string userName)
        {
            int id = -1;

            string query = "SELECT * FROM users WHERE USERNAME = @username";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@username", userName));

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

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

            string query = "SELECT * FROM users WHERE EMAIL = @email";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@email", email));

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

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

            string query = "INSERT INTO users (USERNAME, PASSWORD, FIRSTNAME, LASTNAME, EMAIL, SEX, AGE, STATE) VALUES (@username, @password, @firstname, @lastname, @email, @sex, @age, @state)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@username", user.Username));
                command.Parameters.Add(new MySqlParameter("@password", user.Password));
                command.Parameters.Add(new MySqlParameter("@firstname", user.FirstName));
                command.Parameters.Add(new MySqlParameter("@lastname", user.LastName));
                command.Parameters.Add(new MySqlParameter("@email", user.Email));
                command.Parameters.Add(new MySqlParameter("@sex", user.Sex));
                command.Parameters.Add(new MySqlParameter("@age", user.Age));
                command.Parameters.Add(new MySqlParameter("@state", user.State));

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
