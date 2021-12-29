using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MinesweeperApp.Models
{
    /// <summary>
    /// This class model is designed to be displayed to a public facing API.
    /// </summary>
    public class BoardDTO
    {
        //The database Id for the board.
        [DisplayName("Unique Game Id")]
        public int Id { get; set; }
        
        //Difficulty level for the board
        [DisplayName("Difficult Level (Easy = 1, Medium = 2, Hard = 3)")]
        public int Difficulty { get; set; }

        //Time that the game initially started at
        [DisplayName("Time/Date the game was started")]
        public DateTime TimeStarted { get; set; }

        //Current running play time for this game
        [DisplayName("Current running playtime")]
        [DisplayFormat(DataFormatString="{0:hh\\:mm\\:ss}")]
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
