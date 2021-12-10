using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace MinesweeperApp.DatabaseServices
{
    public class GameBoardDAO
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=MinesweeperApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public List<BoardDTO> GetSavesFromUserId(int id)
        { 
            List<BoardDTO> boards = new List<BoardDTO>();

            string query = "SELECT * FROM boards WHERE USER_ID = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while(reader.Read())
                    {
                        boards.Add(new BoardDTO((int)reader["ID"], (int)reader["DIFFICULTY"], (DateTime)reader["TIMESTARTED"], (TimeSpan)reader["TIMEPLAYED"]));
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

        public int SaveBoard(Board board, int userId)
        {
            int results = -1; //Holds the new ID for this particular board or -1 if insert failed

            string query = "INSERT INTO boards (USER_ID, SIZE, DIFFICULTY, NUMBEROFMINES, TIMESTARTED, TIMEPLAYED) OUTPUT INSERTED.ID VALUES (@user, @size, @difficulty, @numberofmines, @timestarted, @timeplayed);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@user", System.Data.SqlDbType.Int).Value = userId;
                command.Parameters.Add("@size", System.Data.SqlDbType.Int).Value = board.Size;
                command.Parameters.Add("@difficulty", System.Data.SqlDbType.Int).Value = board.Difficulty;
                command.Parameters.Add("@numberofmines", System.Data.SqlDbType.Int).Value = board.NumberOfMines;
                command.Parameters.Add("@timestarted", System.Data.SqlDbType.DateTime).Value = board.TimeStarted;
                command.Parameters.Add("@timeplayed", System.Data.SqlDbType.Time).Value = board.TimePlayed;

                try
                {
                    connection.Open();

                    var affectedRow = command.ExecuteScalar();

                    if(affectedRow != null)
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

        public bool SaveCells(Cell cell, int boardId)
        {
            bool success = false;

            string query = "INSERT INTO cells (CELL_ID, VISITED, MINE, FLAGGED, LIVENEIGHBORS, BOARD_ID) VALUES (@id, @visited, @mine, @flagged, @liveneighbors, @boardid)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = cell.Id;
                command.Parameters.Add("@visited", System.Data.SqlDbType.Bit).Value = cell.Visited;
                command.Parameters.Add("@mine", System.Data.SqlDbType.Bit).Value = cell.Mine;
                command.Parameters.Add("@flagged", System.Data.SqlDbType.Bit).Value = cell.Flagged;
                command.Parameters.Add("@liveneighbors", System.Data.SqlDbType.Int).Value = cell.LiveNeighbors;
                command.Parameters.Add("@boardid", System.Data.SqlDbType.Int).Value = boardId;

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
                    Console.WriteLine("Error during cell save. Cell: " + cell.Id + ". " + ex.Message);
                }
            }

            return success;
        }

        public Board LoadBoard(int boardId)
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

                    if(reader.HasRows)
                    {
                        reader.Read();
                        board = new Board();
                        board.Id = boardId;
                        board.Size = (int)reader["SIZE"];
                        board.Difficulty = (int)reader["DIFFICULTY"];
                        board.NumberOfMines = (int)reader["NUMBEROFMINES"];
                        board.TimeStarted = (DateTime)reader["TIMESTATED"];
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

        public Cell[,] LoadCells(Board board)
        {
            Cell[,] cells = new Cell[board.Size, board.Size];

            string query = "SELECT * FROM cells WHERE BOARD_ID = @boardId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.Add("@boardId", System.Data.SqlDbType.Int).Value = board.Id;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Cell cell = new Cell((int)reader["CELL_ID"], (bool)reader["VISITED"], (bool)reader["MINE"], (bool)reader["FLAGGED"], (int)reader["LIVENEIGHBORS"]);
                        cells[cell.Id / board.Size, cell.Id % board.Size] = cell;
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                };
            }

            return cells;
        }
    }
}
