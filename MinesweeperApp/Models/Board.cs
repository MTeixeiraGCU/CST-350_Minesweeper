using System;
using System.Collections.Generic;
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
    }
}
