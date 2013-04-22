using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PartyLeaderboard.Models;

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
    }
}
