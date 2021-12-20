using MinesweeperApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.DatabaseServices
{
    /// <summary>
    /// This interface represents access to the persistence layer for adding and validating a user
    /// </summary>
    public interface IUserDAO
    {
        public bool FindUserByUsernameAndPassword(string userName, string password);
        public int GetIdFromUsername(string userName);
        public int GetIdFromEmail(string email);
        public bool Add(User user);
    }
}
