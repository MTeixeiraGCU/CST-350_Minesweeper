using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// This class handles processing database exchanges for game board data.
    /// </summary>
    public class GameBoardLocalSqlDAO : IGameBoardDAO
    {
        //Connection string to local VS created MySQL database
        private string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinesweeperApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// This method returns all of the found save games in the entire database.
        /// </summary>
        /// <returns>A complete list of save games from the database.</returns>
        public List<Board> GetAll()
        {
            List<Board> boards = new List<Board>();

            string query = "SELECT * FROM boards";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

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

        /// <summary>
        /// This method retrieves all of the save games owned by the given user Id
        /// </summary>
        /// <param name="userId">The user Id to cross check save games for.</param>
        /// <returns>A list of all the save games from a particular user.</returns>
        public List<Board> GetFromUserId(int userId)
        {
            List<Board> boards = new List<Board>();

            string query = "SELECT * FROM boards WHERE USER_ID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = userId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

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

        /// <summary>
        /// This method saves a new board into the database.
        /// </summary>
        /// <param name="board">The board object to enter into the database.</param>
        /// <param name="userId">The user Id to store the given board under.</param>
        /// <returns>Result integer of the newly saved boards unique Id.</returns>
        public int Add(Board board, int userId)
        {
            int results = -1; //Holds the new ID for this particular board or -1 if insert failed

            string query = "INSERT INTO boards (USER_ID, SIZE, DIFFICULTY, NUMBEROFMINES, GRID, TIMESTARTED, TIMEPLAYED) OUTPUT INSERTED.ID VALUES (@user, @size, @difficulty, @numberofmines, @grid, @timestarted, @timeplayed);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@user", System.Data.SqlDbType.Int).Value = userId;
                command.Parameters.Add("@size", System.Data.SqlDbType.Int).Value = board.Size;
                command.Parameters.Add("@difficulty", System.Data.SqlDbType.Int).Value = board.Difficulty;
                command.Parameters.Add("@numberofmines", System.Data.SqlDbType.Int).Value = board.NumberOfMines;
                command.Parameters.Add("@grid", System.Data.SqlDbType.NVarChar).Value = Board.SerializeGridToString(board.Grid, board.Size, board.Size);
                command.Parameters.Add("@timestarted", System.Data.SqlDbType.DateTime).Value = board.TimeStarted;
                command.Parameters.Add("@timeplayed", System.Data.SqlDbType.Time).Value = board.TimePlayed;

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

        /// <summary>
        /// This method updates a board that already exists in the database. Does not check for existence. Currently only updates playtime and grid status.
        /// </summary>
        /// <param name="board">The board information to update the database with.</param>
        /// <returns>Boolean value representing a successful update. true being success and false otherwise.</returns>
        public bool Update(Board board)
        {
            bool results = false;

            string sqlStatement = "UPDATE boards SET GRID = @grid, TIMEPLAYED = @timeplayed WHERE ID = @boardid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.AddWithValue("@boardid", board.Id);
                command.Parameters.Add("@grid", System.Data.SqlDbType.NVarChar).Value = Board.SerializeGridToString(board.Grid, board.Size, board.Size);
                command.Parameters.AddWithValue("@timeplayed", board.TimePlayed);

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

        /// <summary>
        /// This method loads the given board Id and returns it as a newly created Board object
        /// </summary>
        /// <param name="boardId">The Id of the board to load from the database.</param>
        /// <returns>A newly created board object from the recieved data in the database.</returns>
        public Board Get(int boardId)
        {
            Board board = null;

            string query = "SELECT * FROM boards WHERE ID = @boardid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@boardid", System.Data.SqlDbType.Int).Value = boardId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

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

        /// <summary>
        /// This method removes a single board from the database.
        /// </summary>
        /// <param name="boardId">The Id of the board to remove from the database.</param>
        /// <returns>Boolean value representing a successful deletion. true if the board was removed, false otherwise.</returns>
        public bool Delete(int boardId)
        {
            bool isDeleted = false;

            string sqlStatement = "DELETE FROM boards WHERE ID = @boardid";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.AddWithValue("@boardId", boardId);

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

