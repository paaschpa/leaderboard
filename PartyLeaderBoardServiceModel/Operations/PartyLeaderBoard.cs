using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/{PartyId}/leaderboard")]
    public class PartyLeaderBoard : IReturn<PartyLeaderBoard>
    {
        public int PartyId { get; set; }
        public string PartyName { get; set; }
        public double CutLine { get; set; }
        public List<UserTotalScore> UserScores { get; set; }
    }
}
