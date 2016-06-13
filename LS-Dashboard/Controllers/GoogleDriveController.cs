using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LS_Dashboard.Helpers;

namespace LS_Dashboard.Controllers
{
    public class GoogleDriveController : Controller
    {
        // GET: GoogleDrive
        public ActionResult Items()
        {
            var itemsCheck = GoogleDriveHelper.GetItems();
            return Json(itemsCheck,JsonRequestBehavior.AllowGet);
        }
        public ActionResult TechChecks()
        {
            var techCheck = GoogleDriveHelper.GetTechChecks();
            return Json(techCheck,JsonRequestBehavior.AllowGet);
        }
        public ActionResult Vehicles()
        {
            var vehicleCheck = GoogleDriveHelper.GetVehicles();
            return Json(vehicleCheck,JsonRequestBehavior.AllowGet);
        }
    }
}