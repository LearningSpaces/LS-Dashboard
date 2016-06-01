using LS_Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS_Dashboard.Controllers
{
    public class WiWController : Controller
    {
        public ActionResult GetShifts(int? shift)
        {
            if(shift == null)
            {
                return Json(new { Message = "Shift Cannot Be Null"}, JsonRequestBehavior.AllowGet);
            }
            var shifts = WheniWorkHelper.GetShifts(shift.Value);
            return Json(shifts, JsonRequestBehavior.AllowGet);
        }
    }
}