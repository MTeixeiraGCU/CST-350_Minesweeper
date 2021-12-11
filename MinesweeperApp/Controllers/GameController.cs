using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    public class GameController : Controller
    {
        static GameboardBusinessService gbs = new GameboardBusinessService();
        static SavingLoadingService sls = new SavingLoadingService();
        static bool Lost;
        static bool Won;

        private List<int> updatedCells;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBoard(string options)
        {
            gbs.NewGame(options);
            Lost = false;
            Won = false;

            ViewBag.Width = gbs.Size;
            ViewBag.TimeStarted = gbs.GetStartTime();
            return View("GameBoard", gbs.Grid);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);

            //disabled because of the new ajax calls
            //gbs.MakeMove(id);

            ViewBag.Width = gbs.Size;
            return View("GameBoard", gbs.Grid);
        }

        public JsonResult HandleButtonLeftClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);
            updatedCells = new List<int>();

            if (!Lost && !Won)
            {
                Lost = gbs.MakeMove(id, updateCell);
                Won = gbs.CheckForWin();
            }

            //we lost so show the entire grid
            if(Lost)
            {
                gbs.RevealAll(updateCell);
            }

            ViewBag.Width = gbs.Size;
            return  Json(updatedCells);
        }

        public JsonResult HandleButtonRightClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);
            updatedCells = new List<int>();

            if (!Lost && !Won)
            {
                gbs.ToggleFlag(id);
                Won = gbs.CheckForWin();
                updatedCells.Add(id);
            }

            return Json(updatedCells);
        }

        public IActionResult SaveGame()
        {
            int userId = 1; ///////////////// THIS NEEDS TO BE REMOVED ONCE THERE IS SESSIONS, THE INTEGER SHOULD BE CHANGED TO A VALID USER ID UNTIL THEN

            sls.SaveGame(userId, gbs.GameBoard);
            return LoadGame(gbs.GameBoard.Id);
        }

        public IActionResult LoadGame(int boardId)
        {
            gbs.GameBoard = sls.LoadGame(boardId, gbs.GameBoard);

            Lost = false;
            Won = false;

            ViewBag.Width = gbs.Size;
            ViewBag.TimeStarted = gbs.GetStartTime();
            return View("GameBoard", gbs.Grid);
        }

        public IActionResult DeleteGame(int boardId)
        {
            sls.DeleteSaveGame(boardId);

            return SavedGameList();
        }

        public IActionResult SavedGameList()
        {
            int userId = 1; ///////////////// THIS NEEDS TO BE REMOVED ONCE THERE IS SESSIONS, THE INTEGER SHOULD BE CHANGED TO A VALID USER ID UNTIL THEN

            var list = sls.GetGameList(userId);

            return View(list);
        }

        public IActionResult UpdateOneCell(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);

            return PartialView("SingleButton", gbs.Grid.ElementAt(id));
        }
        public IActionResult Winner()
        {
            return View("Winner");
        }

        public IActionResult EndGame()
        {
            return View("EndGame");
        }

        public int CheckGrid()
        {
            if(Won)
            {
                return 0;
            }
            else if(Lost)
            {
                return 1;
            }
            return 2;
        }

        public void updateCell(int id)
        {
            if(!updatedCells.Contains(id))
                updatedCells.Add(id);
        }

    }
}
