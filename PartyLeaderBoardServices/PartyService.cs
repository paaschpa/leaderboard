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
            if (request.PartyId.HasValue)
            {
                var party = DbConnExec<Party>((con) => con.GetById<Party>(request.PartyId));
                return new List<Party>() {party};
            }

            if(request.CommissionerId.HasValue)
            { return DbConnExec<List<Party>>((con) => con.Select<Party>(x => x.CommissionerId == request.CommissionerId)); }

            if (!String.IsNullOrEmpty(request.PartyName))
            {
                var versionIndex = request.PartyName.LastIndexOf("-");
                var versionString = versionIndex > 0
                                        ? request.PartyName.Substring(versionIndex,
                                                                      request.PartyName.Length - versionIndex)
                                        : "0";
                int version = 0;
                int.TryParse(versionString, out version);
 
                return DbConnExec<List<Party>>((con) => con.Select<Party>(x => x.Name == request.PartyName && x.Version == version));
            }

            return null;
        }

        public List<PartyPlayer> Get(PartyPlayers request)
        {
            return DbConnExec<List<PartyPlayer>>( (con) => con.Select<PartyPlayer>(x => x.PartyId == request.PartyId));
        }

        [Authenticate]
        public Party Post(CreateParty request)
        {
            var newParty = request.TranslateTo<Party>();
            var user = base.SessionAs<AuthUserSession>();
            newParty.CommissionerId = int.Parse(user.UserAuthId);
            newParty.CommissionerName = request.CommissionerName ?? user.UserName;
            DbConnExecTransaction((con) =>
                {
                    con.Insert<Party>(newParty);
                    newParty.Id = (int)con.GetLastInsertId();
                    foreach (var player in request.Players)
                    {
                        con.Insert<PartyPlayer>(new PartyPlayer {PartyId = newParty.Id, Name = player});
                    }
                });

            return newParty;
        }
    }
}
