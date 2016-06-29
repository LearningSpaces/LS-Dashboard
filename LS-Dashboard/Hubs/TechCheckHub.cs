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
    [HubName("TechCheckHub")]
    public class TechCheckHub : Hub
    {
        [HubMethodName("GetTechChecks")]
        public List<TechCheckSectorModel> GetTechChecks()
        {
            return GoogleDriveHelper.GetTechChecks();
        }
    }
}