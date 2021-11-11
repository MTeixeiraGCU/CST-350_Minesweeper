using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserLogin userLogin)
        {
            LoginBusinessService lbs = new LoginBusinessService();
            if(lbs.ValidateLogin(userLogin))
            {
                return View("LoginSuccess", userLogin);
            }
            else
            {
                return View("LoginFailure", userLogin);
            }
        }
    }
}
