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
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderboard.Controllers
{
    public class PartyController : ServiceStackController<AuthUserSession>
    {
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult List()
        {
            var userId = int.Parse(AuthSession.Id);
            var parties = new List<Party>();
            using (var service = AppHost.Resolve<PartyService>())
            {
                parties = service.Get(new Parties {CommissionerId = userId});
            }

            return View(parties);
        }

        public ActionResult Index(int id)
        {
            dynamic viewModel = new ExpandoObject();
            viewModel.PartyId = id;
            return View(viewModel);
        }
    }
}
