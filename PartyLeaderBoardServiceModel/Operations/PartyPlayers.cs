using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Party/{PartyId}/Players")]
    public class PartyPlayers
    {
        public int PartyId { get; set; }
    }
}
