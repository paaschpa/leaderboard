using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/score/{Name}", "GET")]
    public class UserScores
    {
        public string Name { get; set; }
    }
}
