﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;
using ServiceStack.ServiceHost;

namespace PartyLeaderBoardServiceModel.Operations
{
    [Route("/Party", "POST")]
    public class CreateParty
    {
        public string Name { get; set; }
        public DateTime? PartyDate { get; set; }
        public int CommissionerId { get; set; }
        public string CommissionerName { get; set; }
    }
}
