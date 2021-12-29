using MinesweeperApp.Models;
using System.Collections.Generic;

namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// This interface represents a persistence layer connection to create, add, update and delete a Gameboard model.
    /// </summary>
    public interface IGameBoardDAO
    {
        /// <summary>
        /// This method returns all of the found save games in the persistence layer.
        /// </summary>
        /// <returns>A complete list of all save games.</returns>
        public List<Board> GetAll();

        /// <summary>
        /// This method loads the given board Id and returns it as a newly created Board object
        /// </summary>
        /// <param name="boardId">The Id of the board to load from the database.</param>
        /// <returns>A newly created board object from the recieved data in the database.</returns>
        public Board Get(int boardId);

        /// <summary>
        /// This method retrieves all of the save games owned by the given user Id
        /// </summary>
        /// <param name="userId">The user Id to cross check the save games against.</param>
        /// <returns>A list of all the save games from a particular user.</returns>
        public List<Board> GetFromUserId(int userId);

        /// <summary>
        /// This method saves a new board into the persistence layer.
        /// </summary>
        /// <param name="board">The board object to enter into persistence.</param>
        /// <param name="userId">The user Id to store the given board under.</param>
        /// <returns>Result integer of the newly saved boards unique Id.</returns>
        public int Add(Board board, int userId);

        /// <summary>
        /// This method updates a board that already exists. Does not check for existence. Currently only updates playtime and grid status.
        /// </summary>
        /// <param name="board">The board information to update with.</param>
        /// <returns>Boolean value representing a successful update. true being success and false otherwise.</returns>
        public bool Update(Board board);

        /// <summary>
        /// This method removes a single board from the persistence layer.
        /// </summary>
        /// <param name="boardId">The Id of the board to remove.</param>
        /// <returns>Boolean value representing a successful deletion. true if the board was removed, false otherwise.</returns>
        public bool Delete(int boardId);

    }
}
