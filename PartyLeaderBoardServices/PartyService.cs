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
        public List<Party> Get(Parties request)
        {
            return DbConnExec<List<Party>>((con) => con.Select<Party>(x => x.CommissionerId == request.CommissionerId));
        }

        [Authenticate]
        public Party Post(CreateParty request)
        {
            var newParty = request.TranslateTo<Party>();
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
