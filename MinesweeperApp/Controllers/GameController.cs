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

        private List<int> updatedCells;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBoard(string options)
        {
            gbs.NewGame(options);

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

            gbs.MakeMove(id, updateCell);

            ViewBag.Width = gbs.Size;
            return  Json(updatedCells);
        }

        public JsonResult HandleButtonRightClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);
            updatedCells = new List<int>();

            gbs.ToggleFlag(id);
            updatedCells.Add(id);

            return Json(updatedCells);
        }

        public IActionResult UpdateOneCell(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);

            return PartialView("SingleButton", gbs.Grid.ElementAt(id));
        }

        public int CheckGrid(int id)
        {
            if (gbs.CheckForWin(id))
            {
                return 0;
            } else if (gbs.CheckForLoss(id))
            {
                return 1;
            }
            return 2;
        }

        public void updateCell(int id)
        {
            updatedCells.Add(id);
        }

        public IActionResult Winner()
        {
            return View("Winner");
        }
    }
}
