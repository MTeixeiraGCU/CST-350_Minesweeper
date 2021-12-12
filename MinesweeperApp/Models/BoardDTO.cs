﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    /// <summary>
    /// This class model is designed to be displayed to a public facing API.
    /// </summary>
    public class BoardDTO
    {
        //The database Id for the board.
        [DisplayName("Id")]
        public int Id { get; set; }
        
        //Difficulty level for the board
        [DisplayName("Difficult Level(1 - 3)")]
        public int Difficulty { get; set; }

        //Time that the game initially started at
        [DisplayName("Time/Date the game was started")]
        public DateTime TimeStarted { get; set; }

        //Current running play time for this game
        [DisplayName("Current playtime")]
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
