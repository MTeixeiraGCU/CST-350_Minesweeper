using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MinesweeperApp.Models
{
    /// <summary>
    /// This class model holds the information for a single user account
    /// </summary>
    public class User
    {
        //The uniques Id for this user
        public int Id { get; set; }

        //The user's First Name
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string FirstName { get; set; }

        //The user's Last Name
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string LastName { get; set; }

        //The user's associated gender preference
        [Required]
        public string Sex { get; set; }

        //How old the user is
        [Required]
        [Range(5, 120)]
        public int Age { get; set; }

        //Which state the user resides in
        public string State { get; set; }

        //Uniques email for contacting this user
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email address format!")]
        public string Email { get; set; }

        //Unique user name to be used as the user's login identification
        [Required]
        public string Username { get; set; }

        //Password associated with this user's account
        [Required]
        public string Password { get; set; }

        public User(int id, string firstName, string lastName, string sex, int age, string state, string email, string username, string password)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Sex = sex;
            Age = age;
            State = state;
            Email = email;
            Username = username;
            Password = password;
        }

        public User()
        {
        }

        public override string ToString()
        {
            return "Username: " + Username + " Password: " + Password + " User ID: " + Id;
        }
    }

    /// <summary>
    /// This enum represents a gender selection
    /// </summary>
    public enum Gender
    {
        Other = 0,
        Female = 1,
        Male = 2
    }

    /// <summary>
    /// This enum holds all the possible states for selection
    /// </summary>
    public enum States
    {
        None,
        [Description("Alabama")]
        AL,
        [Description("Alaska")]
        AK,
        [Description("Arkansas")]
        AR,
        [Description("Arizona")]
        AZ,
        [Description("California")]
        CA,
        [Description("Colorado")]
        CO,
        [Description("Connecticut")]
        CT,
        [Description("D.C.")]
        DC,
        [Description("Delaware")]
        DE,
        [Description("Florida")]
        FL,
        [Description("Georgia")]
        GA,
        [Description("Hawaii")]
        HI,
        [Description("Iowa")]
        IA,
        [Description("Idaho")]
        ID,
        [Description("Illinois")]
        IL,
        [Description("Indiana")]
        IN,
        [Description("Kansas")]
        KS,
        [Description("Kentucky")]
        KY,
        [Description("Louisiana")]
        LA,
        [Description("Massachusetts")]
        MA,
        [Description("Maryland")]
        MD,
        [Description("Maine")]
        ME,
        [Description("Michigan")]
        MI,
        [Description("Minnesota")]
        MN,
        [Description("Missouri")]
        MO,
        [Description("Mississippi")]
        MS,
        [Description("Montana")]
        MT,
        [Description("North Carolina")]
        NC,
        [Description("North Dakota")]
        ND,
        [Description("Nebraska")]
        NE,
        [Description("New Hampshire")]
        NH,
        [Description("New Jersey")]
        NJ,
        [Description("New Mexico")]
        NM,
        [Description("Nevada")]
        NV,
        [Description("New York")]
        NY,
        [Description("Oklahoma")]
        OK,
        [Description("Ohio")]
        OH,
        [Description("Oregon")]
        OR,
        [Description("Pennsylvania")]
        PA,
        [Description("Rhode Island")]
        RI,
        [Description("South Carolina")]
        SC,
        [Description("South Dakota")]
        SD,
        [Description("Tennessee")]
        TN,
        [Description("Texas")]
        TX,
        [Description("Utah")]
        UT,
        [Description("Virginia")]
        VA,
        [Description("Vermont")]
        VT,
        [Description("Washington")]
        WA,
        [Description("Wisconsin")]
        WI,
        [Description("West Virginia")]
        WV,
        [Description("Wyoming")]
        WY
    }
}
