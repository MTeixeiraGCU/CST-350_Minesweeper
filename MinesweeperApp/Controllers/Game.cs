using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    public class Game : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateBoard()
        {
            return View();
        }
    }
}
