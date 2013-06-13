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

            if (!String.IsNullOrEmpty(request.PartySlug))
            {
                return DbConnExec<List<Party>>((con) => con.Select<Party>(x => x.Slug == request.PartySlug));
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
            var version = 0;
            newParty.CommissionerId = int.Parse(user.UserAuthId);
            newParty.CommissionerName = request.CommissionerName ?? user.UserName;

            var existingPartyWithName =
                DbConnExec<Party>((con) => con.Select<Party>(x => x.Name == request.Name).OrderByDescending(x => x.Version).FirstOrDefault());

            if (existingPartyWithName != null)
            {
                version = existingPartyWithName.Version + 1;
                newParty.Version = version;
            }

            newParty.Slug = BuildSlug(newParty.Name, version);
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

        private string BuildSlug(string name, int version)
        {
            var slug = name.Replace(" ", "-");
            if (version != 0)
            {
                slug += "-" + version.ToString();
            }

            return slug;
        }
    }
}
