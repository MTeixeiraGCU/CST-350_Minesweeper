using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    /// <summary>
    /// This class service is designed to handle logic for user login and validation.
    /// </summary>
    public class LoginBusinessService
    {
        //Database service object for users
        UserDAO userDAO = new UserDAO();

        /// <summary>
        /// This method takes in a user object and attempts to validate thier credintials against the database.
        /// </summary>
        /// <param name="user">A user object containing the login information to process.</param>
        /// <returns>Boolean value of true if the user was validated, false otherwise.</returns>
        public bool ValidateLogin(User user)
        {
            if (userDAO.FindUserByUsernameAndPassword(user.Username, user.Password))
            {
                user.Id = userDAO.GetIdFromUsername(user.Username);
                return true;
            }
            return false;
        }
    }
}
