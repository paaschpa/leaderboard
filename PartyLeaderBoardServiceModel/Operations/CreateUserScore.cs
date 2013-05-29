using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Userscore", "POST")]
    public class CreateUserScore
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Notes { get; set; }
    }
}
