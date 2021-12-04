using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class Cell
    {
        public int Id { get; set; }
        public bool Visited { get; set; }
        public bool Mine { get; set; }
        public bool Flagged { get; set; }
        public int LiveNeighbors { get; set; }

        public Cell()
        {
        }

        public Cell(int id, bool visited = false, bool mine = false, bool flagged = false, int liveNeighbors = 0)
        {
            Id = id;
            Visited = visited;
            Mine = mine;
            Flagged = flagged;
            LiveNeighbors = liveNeighbors;
        }
    }
}
