using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyLeaderBoardServiceModel.Operations
{
    public class CreateParty
    {
        public string Name { get; set; }
        public DateTime? PartyDate { get; set; }
        public int CommissionerId { get; set; }
        public string CommissionerName { get; set; }
    }
}
