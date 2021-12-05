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
            return View("GameBoard", gbs.Grid);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);

            gbs.MakeMove(id);

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
            updatedCells.Add(id);
        }

    }
}
