using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyLeaderBoardServiceModel;
using PartyLeaderBoardServiceModel.Operations;
using PartyLeaderBoardServices;
using PartyLeaderboard.App_Start;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderboard.Controllers
{
    public class PartyController : ServiceStackController<AuthUserSession>
    {
        [Authenticate]
        public ActionResult Create()
        {
            return View();
        }

        [Authenticate]
        public ActionResult List()
        {
            var userId = int.Parse(AuthSession.UserAuthId);
            var parties = new List<Party>();
            using (var service = AppHost.Resolve<PartyService>())
            {
                parties = service.Get(new Parties {CommissionerId = userId});
            }

            return View(parties);
        }

        public ActionResult LeaderBoard(string id)
        {
            Party party = null;
            Parties request = null;
            int partyId;
            if (int.TryParse(id, out partyId) == true)
            {
                request = new Parties() {PartyId = partyId};
            }
            else
            {
                request = new Parties() {PartyName = id};
            }

            using (var service = AppHost.Resolve<PartyService>())
            {
                party = service.Get(request).FirstOrDefault();
            }

            dynamic viewModel = new ExpandoObject();
            viewModel.PartyId = party.Id;
            viewModel.IsCommissioner = false;

            if (AuthSession.UserAuthId != null)
            {
                viewModel.IsCommissioner = int.Parse(AuthSession.UserAuthId) == party.CommissionerId;
            }

            return View(viewModel);
        }
    }
}
