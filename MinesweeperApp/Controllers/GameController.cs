using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    public class GameController : Controller
    {
        static Board GameBoard = new Board();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBoard(string options)
        {
            //create a new board
            GameBoard.CreateNewBoard(options);

            ViewBag.Width = GameBoard.Size;
            return View("GameBoard", GameBoard.Grid);
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            int id = int.Parse(buttonNumber);
            GameBoard.MakeMove(id);

            ViewBag.Width = GameBoard.Size;
            return View("GameBoard", GameBoard.Grid);
        }
    }
}
