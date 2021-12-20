using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MinesweeperApp.Controllers
{
    /// <summary>
    /// This class is a login authorization filter. It is used to detect for a valid login situation.
    /// </summary>
    internal class CustomAuthorizationAttribute : Attribute, IAuthorizationFilter
    {
        public bool LogOutRequired { get; set; }

        /// <summary>
        /// This method checks for a valid user id in the sessions variables. If one does not exist, it redirects to the login page.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            int? userId = context.HttpContext.Session.GetInt32("userId");

            if (userId == null && !LogOutRequired)
                context.Result = new RedirectResult("/Login");
            else if (userId != null && LogOutRequired)
                context.Result = new RedirectResult("/Login/Logout?message=You must log out before you can do that!");
        }
    }
}
