using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return Register(new User());
        }

        public IActionResult Register(User user)
        {
            return View("Index");
        }

        public IActionResult ProcessRegistration(User user)
        {
            return Register(user);
        }
    }
}
