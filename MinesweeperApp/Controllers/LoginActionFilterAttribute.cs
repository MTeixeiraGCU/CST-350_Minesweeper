using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MinesweeperApp.Models;
using MinesweeperApp.Utility;
using System;

namespace MinesweeperApp.Controllers
{
    internal class LoginActionFilterAttribute : Attribute, IActionFilter
    {
        /// <summary>
        /// Logs the parameters entered into the Process Login
        /// method in the Login Controller and that the action
        /// has been executed whether pass or fail
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            User user = (User)((Controller)context.Controller).ViewData.Model;

            MyLogger.GetInstance().Info("Parameters: " + user.ToString());
            MyLogger.GetInstance().Info("Exiting ProcessLogin");

        }

        /// <summary>
        /// Logs that ProcessLogin has been started
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            MyLogger.GetInstance().Info("Entering ProcessLogin");
        }
    }
}