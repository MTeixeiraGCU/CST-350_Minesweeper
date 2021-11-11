using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.DatabaseServices
{
    public class UserDAO
    {
        public bool FindUserByUsernameAndPassword(UserLogin userLogin)
        {
            if (userLogin.Username == "Steve" && userLogin.Password == "123qwe")
                userLogin.Id = 0;
            return userLogin.Id == -1 ? false : true;
        }
    }
}
