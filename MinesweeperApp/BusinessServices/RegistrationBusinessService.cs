using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.BusinessServices
{
    public class RegistrationBusinessService
    {
        UserDAO userDAO = new UserDAO();

        public bool RegisterUser(User user)
        {
            if (userDAO.Add(user))
            {
                user.Id = userDAO.GetIdFromUsername(user.Username);
                return true;
            }
            return false;
        }

        public bool CheckUsernameAvailability(User user)
        {
            if (userDAO.GetIdFromUsername(user.Username) == -1)
                return true;
            return false;
        }

        public bool CheckEmailAvailability(User user)
        {
            if (userDAO.GetIdFromEmail(user.Email) == -1)
                return true;
            return false;
        }
    }
}
