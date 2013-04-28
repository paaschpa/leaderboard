using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PartyLeaderBoardServiceModel
{
    public class UserTotalScore
    {
        public string Name { get; set; }
        public int TotalScore { get; set; }
        public int Ranking { get; set; }
        public double RelationToCut { get; set; }
    }
}
