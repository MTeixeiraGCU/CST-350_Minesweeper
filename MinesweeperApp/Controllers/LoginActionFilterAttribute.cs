using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MinesweeperApp.Models;
using MinesweeperApp.Utility;
using System;

namespace MinesweeperApp.Controllers
{
    internal class LoginActionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
            User user = (User)((Controller)context.Controller).ViewData.Model;

            MyLogger.GetInstance().Info("Parameters: " + user.ToString());
            MyLogger.GetInstance().Info("Exiting ProcessLogin");

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            MyLogger.GetInstance().Info("Entering ProcessLogin");
        }
    }
}