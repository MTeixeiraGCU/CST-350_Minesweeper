using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MinesweeperApp.Models;
using System.Diagnostics;

namespace MinesweeperApp.Controllers
{
    /// <summary>
    /// This class controller is the intial entry point for the application.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This is the intial entry route for the application
        /// </summary>
        /// <returns>A view of the main home page for the application.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Routing path for the about page of the application.
        /// </summary>
        /// <returns>A view containing the about page.</returns>
        public IActionResult About()
        {
            return View();
        }

        //Visual Studio auto generated Error handeling
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
