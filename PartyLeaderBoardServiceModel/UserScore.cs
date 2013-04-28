using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace PartyLeaderBoardServiceModel
{
    [Alias("UserScores")]
    public class UserScore
    {
        public int Id { get; set; }
        public int PartyId { get; set; }
        public int PlayerId { get; set; }
        public int Score { get; set; }
        public DateTime ScoreDate { get; set; }
        public string Notes { get; set; }
    }
}
