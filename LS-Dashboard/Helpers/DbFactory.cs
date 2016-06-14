using LS_Dashboard.Models;
using LS_Dashboard.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Helpers
{
    public static class DbFactory
    {
        public static IncidentContext getDb()
        {
            return new IncidentContext(Settings.ConnectionString);
        }
    }
}