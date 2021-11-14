using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    public class LoginBusinessService
    {
        UserDAO userDAO = new UserDAO();

        public bool ValidateLogin(User userLogin)
        {
            if (userDAO.FindUserByUsernameAndPassword(userLogin))
            {
                userLogin.Id = userDAO.GetIdFromUsername(userLogin.Username);
                return true;
            }
            return false;
        }
    }
}
