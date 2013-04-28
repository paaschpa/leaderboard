using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PartyLeaderBoardServiceModel;
using PartyLeaderBoardServiceModel.Operations;
using PartyLeaderboardServices;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderBoardServices
{
    public class PartyService : LeaderBoardServiceBase
    {
        [Authenticate]
        public Party Post(CreateParty reqeust)
        {
            var newParty = reqeust.TranslateTo<Party>();
            var user = Session.Get<AuthUserSession>(SessionFeature.GetSessionKey());
            newParty.CommissionerId = int.Parse(user.Id); 
            DbConnExecTransaction((con) =>
                {
                    con.Insert<Party>();
                    newParty.Id = (int)con.GetLastInsertId();
                });

            return newParty;
        }
    }
}
