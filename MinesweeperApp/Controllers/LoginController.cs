using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    /// <summary>
    /// This class controller handles routing for the login module
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// Routing path for the intial login page.
        /// </summary>
        /// <returns>A view containing form fields for user login.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This routing path takes in user entered data and attempts to log the user into the system.
        /// </summary>
        /// <param name="user">The user information recieved from the request.</param>
        /// <returns>A view based on failure or success of the login attempt.</returns>
        public IActionResult ProcessLogin(User user)
        {
            LoginBusinessService lbs = new LoginBusinessService();

            if(lbs.ValidateLogin(user))
            {
                return View("LoginSuccess", user);
            }
            else
            {
                return View("LoginFailure", user);
            }
        }
    }
}
