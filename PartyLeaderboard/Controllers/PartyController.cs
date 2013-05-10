using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiceStack.Mvc;
using ServiceStack.OrmLite;
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderboard.Controllers
{
    public class PartyController : ServiceStackController<AuthUserSession>
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult List()
        {
            var userName = AuthSession.UserName;

            return View();
        }
    }
}
