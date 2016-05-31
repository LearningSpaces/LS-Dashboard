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
        public ActionResult ItemCheck()
        {
            var itemsCheck = GoogleDriveHelper.GetItemCheck();
            return Json(itemsCheck,JsonRequestBehavior.AllowGet);
        }
        public ActionResult TechCheck()
        {
            var techCheck = GoogleDriveHelper.GetTechCheck();
            return Json(techCheck,JsonRequestBehavior.AllowGet);
        }
        public ActionResult VehicleCheck()
        {
            var vehicleCheck = GoogleDriveHelper.GetVehicleCheck();
            return Json(vehicleCheck,JsonRequestBehavior.AllowGet);
        }
    }
}