using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
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
            return View("Index", new User());
        }

        public IActionResult ProcessRegistration(User user)
        {
            RegistrationBusinessService rbs = new RegistrationBusinessService();

            //Check for duplicate username
            if(!rbs.CheckUsernameAvailability(user))
            {
                ModelState.AddModelError("Username", "That username has already been taken!");
            }
            //Check for duplicate email
            if(!rbs.CheckEmailAvailability(user))
            {
                ModelState.AddModelError("Email", "That email has been used for another account already!");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", new User());
            }

            //register user
            if (rbs.RegisterUser(user))
            {
                return View("RegistrationSuccess", user);
            }
            else
            {
                return View("RegistrationFailure", user);
            }
        }
    }
}
