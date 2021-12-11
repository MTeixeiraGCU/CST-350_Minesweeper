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

    [ApiController]
    [Route("api")]
    public class GameAPIController : ControllerBase
    {

        SavingLoadingService sls;

        public GameAPIController()
        {
            sls = new SavingLoadingService();
        }

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

        [HttpGet("showSavedGames/{boardId}")]
        [ResponseType(typeof(BoardDTO))]
        public ActionResult<BoardDTO> ShowSavedGames(int boardId)
        {
            Board board = sls.GetSaveGame(boardId);
            BoardDTO boardDTO = new BoardDTO(board.Id, board.Difficulty, board.TimeStarted, board.TimePlayed);
            return boardDTO;
        }

        [HttpGet("deleteOneGame/{boardId}")]
        [ResponseType(typeof(bool))]
        public bool DeleteGame(int boardId)
        {
            return sls.DeleteSaveGame(boardId);
        }
    }
}
