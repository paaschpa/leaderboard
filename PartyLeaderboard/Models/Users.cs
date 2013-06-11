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
                     {1, "Alex"},
                     {2, "Haxel"},
                     {3, "J"}, 
                     {4, "Wems"}, 
                     {5,"Andrew"},
                     {6, "Justin"}, 
                     {7, "Clayton"}
                };
        }
    }
}