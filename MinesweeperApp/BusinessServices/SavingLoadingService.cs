using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    public class SavingLoadingService
    {
        private static GameBoardDAO gbDAO = new GameBoardDAO();

        /// <summary>
        /// This method will save or update a board to the database.
        /// </summary>
        /// <param name="userId">The user Id to save the game under</param>
        /// <param name="board">The board object to save to the database.</param>
        public void SaveGame(int userId, Board board)
        {
            //Update the play time
            board.TimePlayed = DateTime.Now - board.CurrentStartTime;

            //Check for new or previous game save. The board Id will be -1 for new games.
            if (board.Id >= 0)
            {
                //update previous save
                gbDAO.UpdateSavedBoard(board);
            }
            else
            {
                //new game save
                board.Id = gbDAO.SaveBoard(board, userId);
            }
        }

        /// <summary>
        /// This method attempts to load a game from the database
        /// </summary>
        /// <param name="boardId">The Id of the board to retrieve.</param>
        /// <returns>A newly created board from the database. Will return null if no boards were found.</returns>
        public Board LoadGame(int boardId)
        {
            Board board = gbDAO.LoadBoard(boardId); //may return null

            //timer for the current session
            board.CurrentStartTime = DateTime.Now;
            
            return board;
        }

        /// <summary>
        /// This method grabs a list of games from the database.
        /// </summary>
        /// <param name="userId">The user Id to match games against on the database. Will defualt to everyone if left out.</param>
        /// <returns>A list of games for the given user Id or all saves if no user Id was given.</returns>
        public List<Board> GetGameList(int userId = -1)
        {
            if (userId == -1)
                return gbDAO.GetAllGameSaves();
            return gbDAO.GetSavesFromUserId(userId);
        }

        /// <summary>
        /// This method grabs a specific game from the database.
        /// </summary>
        /// <param name="boardId">The board Id of the game to search for.</param>
        /// <returns>A newly created board object of the game found on the database. Will return null if no board was found.</returns>
        public Board GetSaveGame(int boardId)
        {
            return gbDAO.LoadBoard(boardId);
        }

        /// <summary>
        /// This method removes the given saved game from the database.
        /// </summary>
        /// <param name="boardId">The Id of the board to remove.</param>
        /// <returns>true if the board was removed, false otherwise.</returns>
        public bool DeleteSaveGame(int boardId)
        {
            return gbDAO.DeleteBoard(boardId);
        }
    }
}
