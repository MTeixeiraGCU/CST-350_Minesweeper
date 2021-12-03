using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class Board
    {
        public int Size { get; set; }
        public int Difficulty { get; set; }
        public int NumberOfMines { get; set; }
        public Cell[,] Grid;

        public Board()
        {
        }
    }
}
