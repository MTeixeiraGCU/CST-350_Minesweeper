using System;
using System.Collections.Generic;
using System.IO;
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

        public static string SerializeCellToString(Cell cell)
        {
            StringWriter sw = new StringWriter();
            sw.Write(cell.Id + "&");
            sw.Write(cell.Visited + "&");
            sw.Write(cell.Mine + "&");
            sw.Write(cell.Flagged + "&");
            sw.Write(cell.LiveNeighbors);
            return sw.ToString();
        }

        public static Cell DeserializeCellFromString(string str)
        {
            Cell cell = new Cell();
            string[] data = str.Split('&');
            try
            {
                cell.Id = int.Parse(data[0]);
                cell.Visited = bool.Parse(data[1]);
                cell.Mine = bool.Parse(data[2]);
                cell.Flagged = bool.Parse(data[3]);
                cell.LiveNeighbors = int.Parse(data[4]);
            }
            catch(Exception ex)
            {
                cell = null;
                Console.WriteLine("Error while parsing: " + str + " :" + ex.Message);
            }
            return cell;
        }
    }
}
