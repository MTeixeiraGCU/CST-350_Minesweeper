using MinesweeperApp.DatabaseServices;
using MinesweeperApp.Models;

namespace MinesweeperApp.BusinessServices
{
    /// <summary>
    /// This class service is designed to handle logic for user login and validation.
    /// </summary>
    public class LoginBusinessService
    {
        //Database service object for users
        IUserDAO userDAO = new UserMySqlDAO(); /////////////////////////////// NEEDS TO BE INJECTED LATER //////////////////////////////////////////////

        /// <summary>
        /// This method takes in a user object and attempts to validate thier credintials against the database.
        /// </summary>
        /// <param name="user">A user object containing the login information to process.</param>
        /// <returns>Integer value of the logged in user. Will return -1 if the log in failed.</returns>
        public int ValidateLogin(User user)
        {
            if (userDAO.FindUserByUsernameAndPassword(user.Username, user.Password))
            {
                return userDAO.GetIdFromUsername(user.Username);
            }
            return -1;
        }
    }
}
