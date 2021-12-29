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
        /// <summary>
        /// This method retrieves a user's complete information from the given user name and password.
        /// </summary>
        /// <param name="userName">Unique user name to search for in the database</param>
        /// <param name="password">Associated password to validate user with.</param>
        /// <returns>Boolean value of successful location of user credintials. true if they were found, false otherwise.</returns>
        public bool FindUserByUsernameAndPassword(string userName, string password);

        /// <summary>
        /// This metthod finds the user's unique Id from thier chosen userName.
        /// </summary>
        /// <param name="userName">The name that the user chose during registration.</param>
        /// <returns>The found Id that matches the user's unique userName. returns -1 if no entries were found.</returns>
        public int GetIdFromUsername(string userName);

        /// <summary>
        /// This metthod finds the user's unique Id from thier given email.
        /// </summary>
        /// <param name="email">The email that the user chose during registration.</param>
        /// <returns>The found Id that matches the user's unique email. returns -1 if no entries were found.</returns>
        public int GetIdFromEmail(string email);

        /// <summary>
        /// This method adds a user to the database.
        /// </summary>
        /// <param name="user">The user object with the information to add.</param>
        /// <returns>Boolean value that represents whether the user was successfully added. true if they were added, false otherwise.</returns>
        public bool Add(User user);
    }
}
