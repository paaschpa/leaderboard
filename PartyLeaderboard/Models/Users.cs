using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PartyLeaderboard.Models
{
    public class Users
    {
        public static Dictionary<int, String> GetUsers()
        {
            return new Dictionary<int, string>
                {
                     {1, "Nof"},
                     {2, "RyRy"},
                     {3, "Dee"}, 
                     {4, "Michelle"}, 
                     {5,"Rusty"},
                     {6, "Lana"}
                };
        }
    }
}