using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
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
            using(var db = DbFactory.getDb())
            {
                return View(db.Incidents.ToList());
            }
        }

        public ActionResult Test(byte? filter)
        {
            if (filter == null)
            {
                filter = (byte) WEPAModel.Filters.GREEN | (byte) WEPAModel.Filters.YELLOW | (byte) WEPAModel.Filters.RED;
            }
            var results = WEPAHelper.GetWEPAStatus(filter.Value);
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}