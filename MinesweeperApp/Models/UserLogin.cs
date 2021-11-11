using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Models
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public UserLogin()
        {
            Id = -1;
            Username = "";
            Password = "";
        }
    }
}
