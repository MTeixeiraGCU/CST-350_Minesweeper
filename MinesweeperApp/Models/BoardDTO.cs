using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class BoardDTO
    {
        public int Id { get; set; }
        public int Difficulty { get; set; }
        public DateTime TimeStarted { get; set; }
        public TimeSpan TimePlayed { get; set; }

        public BoardDTO(int id, int difficulty, DateTime timeStarted, TimeSpan timePlayed)
        {
            Id = id;
            Difficulty = difficulty;
            TimeStarted = timeStarted;
            TimePlayed = timePlayed;
        }
    }
}
