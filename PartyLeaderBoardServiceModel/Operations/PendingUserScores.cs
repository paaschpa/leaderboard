using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Party/{PartyId}/PendingScores")]
    public class PendingScores
    {
        public int PartyId { get; set; }
    }
}
