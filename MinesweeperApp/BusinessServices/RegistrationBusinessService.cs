using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;

namespace MinesweeperApp.BusinessServices
{
    /// <summary>
    /// This business service class is designed to handle processing logic for user registration.
    /// </summary>
    public class RegistrationBusinessService
    {
        //Database service object for users
        UserLocalSqlDAO userDAO = new UserLocalSqlDAO(); /////////////////////////////// NEEDS TO BE INJECTED LATER //////////////////////////////////////////////

        /// <summary>
        /// Takes in a user object and attempts to add them to the database. Does not create login status.
        /// </summary>
        /// <param name="user">The complete user object to add to the database.</param>
        /// <returns>true if the user was added, false otherwise.</returns>
        public bool RegisterUser(User user)
        {
            return userDAO.Add(user);
        }

        /// <summary>
        /// This method checks for a duplicate user name in the system.
        /// </summary>
        /// <param name="userName">The user name to match the database against.</param>
        /// <returns>true if a user has registered the given user name already, false otherwise</returns>
        public bool CheckUsernameAvailability(string userName)
        {
            if (userDAO.GetIdFromUsername(userName) == -1)
                return true;
            return false;
        }

        /// <summary>
        /// This method checks for a duplicate email in the system.
        /// </summary>
        /// <param name="email">The email string to search the database for</param>
        /// <returns>true if another user has already registered with the given email, flase otherwise.</returns>
        public bool CheckEmailAvailability(string email)
        {
            if (userDAO.GetIdFromEmail(email) == -1)
                return true;
            return false;
        }
    }
}
