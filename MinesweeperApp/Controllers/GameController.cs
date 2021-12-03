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
            bool lose = gbs.MakeMove(id);
            bool win = gbs.CheckBoardVisits();

            if (lose)
            {
                return View("EndGame");
            } else if (win)
            {
                return View("Winner");
            }

            ViewBag.Width = gbs.Size;
            return View("GameBoard", gbs.Grid);
        }
    }
}
