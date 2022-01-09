using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using MinesweeperApp.Utility;
using NLog;

namespace MinesweeperApp.Controllers
{
    /// <summary>
    /// This class controller handles routing for the login module
    /// </summary>
    public class LoginController : Controller
    {
        private LoginBusinessService lbs;

        public LoginController(LoginBusinessService lbs)
        {
            this.lbs = lbs;
        }

        /// <summary>
        /// Routing path for the intial login page.
        /// </summary>
        /// <returns>A view containing form fields for user login.</returns>
        [CustomAuthorization(LogOutRequired = true)]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This routing path takes in user entered data and attempts to log the user into the system.
        /// </summary>
        /// <param name="user">The user information recieved from the request.</param>
        /// <returns>A view based on failure or success of the login attempt.</returns>
        [CustomAuthorization(LogOutRequired = true)]
        [LoginActionFilter]
        public IActionResult ProcessLogin(User user)
        {
            //validate the user
            user.Id = lbs.ValidateLogin(user);

            //check validation from user id
            if (user.Id != -1)
            {
                //set up session variables
                HttpContext.Session.SetInt32("userId", user.Id);
                HttpContext.Session.SetString("username", user.Username);

                // log username and session ID 
                MyLogger.GetInstance().Info("Login successful: " + user.Username + " | Session ID: " + HttpContext.Session.Id);

                return View("LoginSuccess", user);
            }
            else
            {
                MyLogger.GetInstance().Info("Login failure!");
                return View("LoginFailure", user);
            }
        }

        /// <summary>
        /// This method routes to logout the logout page
        /// </summary>
        /// <returns>A view to the logout page view</returns>
        [CustomAuthorization(LogOutRequired = false)]
        public IActionResult Logout()
        {
            return View();
        }

        /// <summary>
        /// This is a helper method to create a logged out state.
        /// </summary>
        /// <returns>True if there was a suer and they were logged out. False if there was no user to logout.</returns>
        [CustomAuthorization(LogOutRequired = false)]
        public IActionResult ProcessLogout()
        {
            MyLogger.GetInstance().Info("Entering ProcessLogout");

            if (HttpContext.Session.GetInt32("userId") != null)
            {
                // log username and user id being logged out
                MyLogger.GetInstance().Info("Logout user: Username: " + HttpContext.Session.GetString("username") + " | User ID: " + HttpContext.Session.GetInt32("userId"));

                //remove session variables
                HttpContext.Session.Remove("userId");
                HttpContext.Session.Remove("username");
            }
            MyLogger.GetInstance().Info("Exiting ProcessLogout");
            return View("Index");
        }
    }
}
