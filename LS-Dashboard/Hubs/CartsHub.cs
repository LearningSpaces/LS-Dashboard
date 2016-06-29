using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

// SignalR hub for handling the carts' statuses
namespace LS_Dashboard.Hubs
{
    [HubName("CartsHub")]
    public class CartsHub : Hub
    {
        // Method to get all of the carts' statuses
        // Currently uses the GoogleDriveHelper to pull the info from Google Drive
        [HubMethodName("GetCartStatuses")]
        public List<CartModel> GetCartStatuses()
        {
            return GoogleDriveHelper.GetVehicles();
        }
    }
}