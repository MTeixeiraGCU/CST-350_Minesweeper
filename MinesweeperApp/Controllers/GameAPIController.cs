using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Description;

namespace MinesweeperApp.Controllers
{

    /// <summary>
    /// This class controller is to support an public facing access point for game board data. Users can view complete lists of games, single games by Id, and delete games by Id.
    /// </summary>
    [ApiController]
    [Route("api")]
    public class GameAPIController : ControllerBase
    {

        //Business service in charge of loading and saving Board objects.
        SavingLoadingService sls;

        public GameAPIController()
        {
            sls = new SavingLoadingService();
        }

        /// <summary>
        /// Home routing point for the GameBoard API.
        /// </summary>
        /// <returns>A complete list of all games in the database.</returns>
        [HttpGet]
        [ResponseType(typeof(List<BoardDTO>))]
        public IEnumerable<BoardDTO> Index()
        {
            List<Board> boardList = sls.GetGameList();
            IEnumerable<BoardDTO> boardDTOList = from b in boardList
                                         select
                                         new BoardDTO(b.Id, b.Difficulty, b.TimeStarted, b.TimePlayed);
            return boardDTOList;
        }

        /// <summary>
        /// Routing point for showing all games from the currently logged in user. User is hardcoded at the moment to Id 1.
        /// </summary>
        /// <returns>A list containing all save games owned by this user.</returns>
        [HttpGet("showSavedGames")]
        public IEnumerable<BoardDTO> ShowSavedGames()
        {
            int userId = 1; ///////////////// THIS NEEDS TO BE REMOVED ONCE THERE IS SESSIONS, THE INTEGER SHOULD BE CHANGED TO A VALID USER ID UNTIL THEN

            List<Board> boardList = sls.GetGameList(userId);
            IEnumerable<BoardDTO> boardDTOList = from b in boardList
                                                 select
                                                 new BoardDTO(b.Id, b.Difficulty, b.TimeStarted, b.TimePlayed);
            return boardDTOList;
        }

        /// <summary>
        /// Routing point for accessing a single game from the logged in user.
        /// </summary>
        /// <param name="boardId">The Id of the game board to be displayed</param>
        /// <returns>A single board object from the given boardId owned by the currently logged in user</returns>
        [HttpGet("showSavedGames/{boardId}")]
        [ResponseType(typeof(BoardDTO))]
        public ActionResult<BoardDTO> ShowSavedGames(int boardId)
        {
            Board board = sls.GetSaveGame(boardId);
            BoardDTO boardDTO = new BoardDTO(board.Id, board.Difficulty, board.TimeStarted, board.TimePlayed);
            return boardDTO;
        }

        /// <summary>
        /// Routing to delete a single game from the database.
        /// </summary>
        /// <param name="boardId">The game id to remove from the database</param>
        /// <returns>A boolean value with true as the game was successfully removed and false otherwise</returns>
        [HttpGet("deleteOneGame/{boardId}")]
        [ResponseType(typeof(bool))]
        public bool DeleteGame(int boardId)
        {
            return sls.DeleteSaveGame(boardId);
        }
    }
}
