using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LS_Dashboard.Controllers
{
    public class WiWController : Controller
    {
        public ActionResult Shifts(int? shift)
        {
            if(shift == null)
            {
                shift = WhenIWorkHelper.GetShiftFromTime(DateTime.Now);
            }
            var cur_shifts = WhenIWorkHelper.GetShifts(shift.Value);
            var next_shifts = WhenIWorkHelper.GetShifts(shift.Value + 1);
            return Json(new {
                current = new {
                    shifts = cur_shifts,
                    number = shift.Value
                },
                next = new {
                    shifts = next_shifts,
                    number = shift.Value + 1
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}