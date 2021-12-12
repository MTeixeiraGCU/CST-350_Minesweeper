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
