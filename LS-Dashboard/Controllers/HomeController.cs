using LS_Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS_Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using(var db = DbFactory.getDb()) {
                return View(db.Incidents.ToList());
            }
        }

        public ActionResult Test()
        {
            var results = WEPAHelper.GetWEPAStatus(7);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}