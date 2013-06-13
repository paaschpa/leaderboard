using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Parties/{CommissionerId}")]
    public class Parties
    {
        public int? CommissionerId { get; set; }
        public int? PartyId { get; set; }
        public string PartyName { get; set; }
    }
}
