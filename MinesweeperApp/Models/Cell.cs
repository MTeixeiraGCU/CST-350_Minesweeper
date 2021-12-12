using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    /// <summary>
    /// This class model is designed represent a single cell within a minsweeper game
    /// </summary>
    public class Cell
    {
        //The Id of this particular cell. Can also be used to determine position into a two-dimensional grid: row = Id / rowSize, col = Id % colSize
        public int Id { get; set; }
        //Flag to determine if the user has visited this cell yet.
        public bool Visited { get; set; }
        //Flag to determine if this cell is a mine
        public bool Mine { get; set; }
        //Flag to mark whether or not the user as marked this cell as a potential mine
        public bool Flagged { get; set; }
        //Integer describing the number of mines that neighbor this particular cell
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

        /// <summary>
        /// This method takes in a cell and converts it into a serialized string
        /// </summary>
        /// <param name="cell">The cell to serialize. Does not check for a vaild cell</param>
        /// <returns>A string represnting all of the cells data in one line.</returns>
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

        /// <summary>
        /// This method takes in a string representing cell data and converts it back into a cell object
        /// </summary>
        /// <param name="str">The serialized information string</param>
        /// <returns>A complete cell object based on the parsed data. Will return null if the parsing was incomplete or failed.</returns>
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
