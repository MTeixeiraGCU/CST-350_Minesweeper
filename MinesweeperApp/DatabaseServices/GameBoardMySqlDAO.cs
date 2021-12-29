using MinesweeperApp.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;

namespace MinesweeperApp.DatabaseServices
{
    public class GameBoardMySqlDAO : IGameBoardDAO
    {
        //local environment variables
        private static string database_server = Environment.GetEnvironmentVariable("DATABASE_SERVER_NAME");
        private static string database_userId = Environment.GetEnvironmentVariable("DATABASE_USER_ID");
        private static string database_password = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
        private static string database_schema = Environment.GetEnvironmentVariable("DATABASE_SCHEMA");
        private static string database_port = Environment.GetEnvironmentVariable("DATABASE_PORT");

        //MySQL database connection string.
        private static string connectionString = "server=" + database_server + ";UserId=" + database_userId + ";password=" + database_password + ";database=" + database_schema + ";port=" + database_port;

        public List<Board> GetAll()
        {
            List<Board> boards = new List<Board>();

            string query = "SELECT * FROM boards";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Board board = new Board();
                        board.Id = (int)reader["ID"];
                        board.Size = (int)reader["SIZE"];
                        board.Difficulty = (int)reader["DIFFICULTY"];
                        board.NumberOfMines = (int)reader["NUMBEROFMINES"];
                        board.Grid = Board.DeserializeGridFromString((string)reader["GRID"]);
                        board.TimeStarted = (DateTime)reader["TIMESTARTED"];
                        board.TimePlayed = (TimeSpan)reader["TIMEPLAYED"];
                        boards.Add(board);
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }

            return boards;
        }

        public Board Get(int boardId)
        {
            Board board = null;

            string query = "SELECT * FROM boards WHERE ID = @boardid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@boardid", boardId));

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        board = new Board();
                        board.Id = boardId;
                        board.Size = (int)reader["SIZE"];
                        board.Difficulty = (int)reader["DIFFICULTY"];
                        board.NumberOfMines = (int)reader["NUMBEROFMINES"];
                        board.Grid = Board.DeserializeGridFromString((string)reader["GRID"]);
                        board.TimeStarted = (DateTime)reader["TIMESTARTED"];
                        board.TimePlayed = (TimeSpan)reader["TIMEPLAYED"];
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }
            return board;
        }

        public List<Board> GetFromUserId(int userId)
        {
            List<Board> boards = new List<Board>();

            string query = "SELECT * FROM boards WHERE USER_ID = @userid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@userid", userId));

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Board board = new Board();
                        board.Id = (int)reader["ID"];
                        board.Size = (int)reader["SIZE"];
                        board.Difficulty = (int)reader["DIFFICULTY"];
                        board.NumberOfMines = (int)reader["NUMBEROFMINES"];
                        board.Grid = Board.DeserializeGridFromString((string)reader["GRID"]);
                        board.TimeStarted = (DateTime)reader["TIMESTARTED"];
                        board.TimePlayed = (TimeSpan)reader["TIMEPLAYED"];
                        boards.Add(board);
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }

            return boards;
        }

        public int Add(Board board, int userId)
        {
            int results = -1; //Holds the new ID for this particular board or -1 if insert failed

            string query = "INSERT INTO boards (USER_ID, SIZE, DIFFICULTY, NUMBEROFMINES, GRID, TIMESTARTED, TIMEPLAYED) OUTPUT INSERTED.ID VALUES (@user, @size, @difficulty, @numberofmines, @grid, @timestarted, @timeplayed);";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                command.Parameters.Add(new MySqlParameter("@user", userId));
                command.Parameters.Add(new MySqlParameter("@size", board.Size));
                command.Parameters.Add(new MySqlParameter("@difficulty", board.Difficulty));
                command.Parameters.Add(new MySqlParameter("@numberofmines", board.NumberOfMines));
                command.Parameters.Add(new MySqlParameter("@grid", Board.SerializeGridToString(board.Grid, board.Size, board.Size)));
                command.Parameters.Add(new MySqlParameter("@timestarted", board.TimeStarted));
                command.Parameters.Add(new MySqlParameter("@timeplayed", board.TimePlayed));

                try
                {
                    connection.Open();

                    var affectedRow = command.ExecuteScalar();

                    if (affectedRow != null)
                    {
                        results = (int)affectedRow;
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during board save. " + ex.Message);
                }
            }

            return results;
        }

        public bool Update(Board board)
        {
            bool results = false;

            string sqlStatement = "UPDATE boards SET GRID = @grid, TIMEPLAYED = @timeplayed WHERE ID = @boardid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                command.Parameters.Add(new MySqlParameter("@boardid", board.Id));
                command.Parameters.Add(new MySqlParameter("@grid", Board.SerializeGridToString(board.Grid, board.Size, board.Size)));
                command.Parameters.Add(new MySqlParameter("@timeplayed", board.TimePlayed));

                try
                {
                    connection.Open();

                    int affectedRows = command.ExecuteNonQuery();
                    if (affectedRows > 0)
                        results = true;

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return results;
        }

        public bool Delete(int boardId)
        {
            bool isDeleted = false;

            string sqlStatement = "DELETE FROM boards WHERE ID = @boardid";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(sqlStatement, connection);

                command.Parameters.Add(new MySqlParameter("@boardId", boardId));

                try
                {
                    connection.Open();

                    command.ExecuteNonQuery();
                    isDeleted = true;

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return isDeleted;
        }
    }
}
