using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using LS_Dashboard.Helpers;
using LS_Dashboard.Models;

namespace LS_Dashboard.Hubs
{
    [HubName("WiWHub")]
    public class WiWHub : Hub
    {
        [HubMethodName("GetWorkers")]
        public object GetWorkers()
        {
            var shift = WhenIWorkHelper.GetShiftFromTime(DateTime.Now);
            var current = WhenIWorkHelper.GetShifts(shift);
            var next = WhenIWorkHelper.GetShifts(shift + 1);
            return new
            {
                current_shifts = current,
                next_shifts = next
            };
        }

        [HubMethodName("GetWorkersByShift")]
        public List<ShiftModel> GetWorkersByShift(int shift)
        {
            var shifts = WhenIWorkHelper.GetShifts(shift);
            return shifts;
        }

        [HubMethodName("GetWorkersByTime")]
        public List<ShiftModel> GetWorkersByTime(DateTime time)
        {
            var shifts = WhenIWorkHelper.GetShifts(time);
            return shifts;
        }
    }
}