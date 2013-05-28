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
using ServiceStack.Mvc;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.WebHost.Endpoints;

namespace PartyLeaderboard.Controllers
{
    public class HomeController : ServiceStackController<AuthUserSession>
    {
        public ActionResult Index()
        {
            if(UserSession.IsAuthenticated)
            { return RedirectToAction("List", "Party"); }
            
            return View();
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
                    auth.RememberMe = true;
                    var response = service.Authenticate(auth);
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

        public ActionResult LogOut()
        {
            //api logout
            var apiAuthService = AppHostBase.Resolve<AuthService>();
            apiAuthService.RequestContext = System.Web.HttpContext.Current.ToRequestContext();
            apiAuthService.Post(new Auth() { provider = "logout" });
            //forms logout
            return RedirectToAction("Index", "Home");
        }
    }
}
