using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace LS_Dashboard.Hubs
{
    [HubName("CartsHub")]
    public class CartsHub : Hub
    {
        [HubMethodName("GetCartStatuses")]
        public List<CartModel> GetCartStatuses()
        {
            return GoogleDriveHelper.GetVehicles();
        }
    }
}