using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Party/{PartyId}/Score/{Name}", "GET")]
    public class UserScores
    {
        public int PartyId { get; set; }
        public string Name { get; set; }
    }
}
