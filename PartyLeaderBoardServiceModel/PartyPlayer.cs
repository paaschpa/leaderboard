using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace PartyLeaderBoardServiceModel
{
    [Alias("PartyPlayers")]
    public class PartyPlayer
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public int PartyId { get; set; }
        public string Name { get; set; }
    }
}
