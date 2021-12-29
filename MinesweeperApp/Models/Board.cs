using System;
using System.IO;

namespace MinesweeperApp.Models
{
    /// <summary>
    /// This class model represents a minesweeper game board.
    /// </summary>
    public class Board
    {
        //This is the unique Id for the board and is used for saving and loading 
        public int Id { get; set; }
        //This is the square size of the minesweeper grid. Size x Size will end up being the dimensions
        public int Size { get; set; }
        //This is the difficulty level for the current board. Currently effects number of mines.
        public int Difficulty { get; set; }
        //This is the total number of mines hidden on the board
        public int NumberOfMines { get; set; }
        //The Cell Grid that holds information for each indivdual part of the board
        public Cell[,] Grid { get; set; }
        //Stored time for the initial game creation
        public DateTime TimeStarted { get; set; }
        //Local stored time for the current run between saves
        public DateTime CurrentStartTime { get; set; }
        //Total amount of time that the user has spent playing this particular board
        public TimeSpan TimePlayed { get; set; }

        public Board()
        {
            Id = -1; //used to set this board as a new game
        }

        /// <summary>
        /// This method takes in a grid and serialize the size and each individual cell into a string
        /// </summary>
        /// <param name="grid">the [row, col] grid of Cells to serialize</param>
        /// <param name="row">The number of rows in the grid</param>
        /// <param name="col">The number of columns in the grid.</param>
        /// <returns>A string representation of the given grid.</returns>
        public static string SerializeGridToString(Cell[,] grid, int row, int col)
        {
            StringWriter sw = new StringWriter();

            //save grid size to first line
            sw.WriteLine(row + "," + col);

            //loop through each cell per line
            for(int i = 0;i < row;i++)
            {
                for(int j = 0;j < col;j++)
                {
                    sw.WriteLine(Cell.SerializeCellToString(grid[i, j]));
                }
            }

            return sw.ToString();
        }

        /// <summary>
        /// This method takes in a string and parses it back into a grid of type Cell[,]
        /// </summary>
        /// <param name="str">The parsable information containing the size on the first line and #,# each line after an individual cell.</param>
        /// <returns>A completed Cell[,] grid</returns>
        public static Cell[,] DeserializeGridFromString(String str)
        {
            StringReader sr = new StringReader(str);
            int row = 0, col = 0;

            //get size of grid, separation is with a ','
            string data = sr.ReadLine();
            try
            {
                string[] temp = data.Split(',');
                row = int.Parse(temp[0]);
                col = int.Parse(temp[1]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error while parsing grid size infomartion: " + ex.Message);
            }

            //parse each cell out of the string
            Cell[,] grid = new Cell[row, col];
            while(sr.Peek() != -1)
            {
                data = sr.ReadLine();
                Cell cell = Cell.DeserializeCellFromString(data);
                grid[cell.Id / row, cell.Id % col] = cell;
            }

            return grid;
        }
    }
}
