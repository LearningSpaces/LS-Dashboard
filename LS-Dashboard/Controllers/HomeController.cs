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

        public ActionResult Test()
        {
            var res = CherwellHelper.getTasks();

            return Json(new { result = res }, JsonRequestBehavior.AllowGet);
        }
    }
}