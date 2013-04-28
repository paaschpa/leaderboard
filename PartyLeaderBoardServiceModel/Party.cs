using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace PartyLeaderBoardServiceModel
{
    [Alias("Parties")]
    public class Party
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public DateTime PartyDate { get; set; }
        public int CommissionerId { get; set; }
        public string CommissionerName { get; set; }
    }
}
