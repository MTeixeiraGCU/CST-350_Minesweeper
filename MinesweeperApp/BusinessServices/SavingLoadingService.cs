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

        //this method will save or update the current game with the given userId
        public void SaveGame(int userId, Board board)
        {
            board.TimePlayed = DateTime.Now - board.CurrentStartTime;
            if (board.Id >= 0)
            {
                //update previous time
                gbDAO.UpdateSavedBoard(board.Id, board.TimePlayed);

                //remove old cells so that new ones can be added
                gbDAO.DeleteCells(board.Id);
            }
            else
            {
                //new game save
                board.Id = gbDAO.SaveBoard(board, userId);
            }

            if (board.Id >= 0) //we have a good save in the database
            {
                for (int i = 0; i < board.Size * board.Size; i++)
                {
                    gbDAO.SaveCells(board.Grid[i / board.Size, i % board.Size], board.Id);
                }
            }
        }

        //This method attempts to load a game from the given id
        public Board LoadGame(int boardId, Board board)
        {
            board = gbDAO.LoadBoard(boardId); //may return null
            if (board != null)
                board.Grid = gbDAO.LoadCells(board);

            //timer for the current session
            board.CurrentStartTime = DateTime.Now;
            
            return board;
        }

        //grabs a list of games from the specified user
        public List<BoardDTO> GetGameList(int userId)
        {
            return gbDAO.GetSavesFromUserId(userId);
        }

        //grabs a specific game from the sepcified user
        public BoardDTO GetSaveGame(int userId, int gameId)
        {
            return gbDAO.GetSaveFromUserIdAndGameId(userId, gameId);
        }

        //removes the given saved game from the database
        public void DeleteSaveGame(int gameId)
        {
            gbDAO.DeleteCells(gameId);
            gbDAO.DeleteBoard(gameId);
        }
    }
}
