using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyLeaderboard.App_Start;
using PartyLeaderboard.Models;
using ServiceStack;
using ServiceStack.Common.Web;
using ServiceStack.ServiceInterface.Auth;

namespace PartyLeaderboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            dynamic viewModel = new ExpandoObject();
            viewModel.Users = Users.GetUsers().Select(x => new {id = x.Key, name = x.Value });
            return View(viewModel);
        }

        public ActionResult Rules()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Auth auth) //Not very intuitive...
        {
            using (var service = AppHost.Resolve<AuthService>())
            {
                try
                {
                    service.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
                    var response = service.Post(auth);
                    return RedirectToAction("List", "Party");
                }
                catch (HttpError)
                {
                    
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View();
        }
    }
}
