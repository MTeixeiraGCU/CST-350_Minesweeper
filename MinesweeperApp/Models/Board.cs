using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class Board
    {
        public int Id { get; set; }
        public int Size { get; set; }
        public int Difficulty { get; set; }
        public int NumberOfMines { get; set; }
        public Cell[,] Grid { get; set; }
        public DateTime TimeStarted { get; set; }
        public DateTime CurrentStartTime { get; set; }
        public TimeSpan TimePlayed { get; set; }

        public Board()
        {
            Id = -1; //used to see if this is a new game or previous save
        }

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

        public static Cell[,] DeserializeGridFromString(String str)
        {
            StringReader sr = new StringReader(str);
            int row = 0, col = 0;

            //get size of grid
            string data = sr.ReadLine();
            try
            {
                string[] temp = data.Split(',');
                row = int.Parse(temp[0]);
                col = int.Parse(temp[1]);
            }
            catch(Exception ex)
            {

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
