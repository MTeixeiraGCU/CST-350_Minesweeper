﻿using Microsoft.AspNetCore.Mvc;
using MinesweeperApp.BusinessServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Controllers
{
    /// <summary>
    /// This class controller is designed to process routing for the registration module
    /// </summary>
    public class RegistrationController : Controller
    {
        /// <summary>
        /// Intial registration module entry point route.
        /// </summary>
        /// <returns>A view of the initial form for user registration.</returns>
        public IActionResult Index()
        {
            return View("Index", new User());
        }

        /// <summary>
        /// This routing path attempts to process a user's registration information and add them into the system.
        /// </summary>
        /// <param name="user">The user information recieved from the request to be processed.</param>
        /// <returns>A view based on successful registration or returns to the registration form with errors.</returns>
        public IActionResult ProcessRegistration(User user)
        {
            //Registration business service object creation
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

            //check for any remaing errors in the form
            if (!ModelState.IsValid)
            {
                //return to the form with errors
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
