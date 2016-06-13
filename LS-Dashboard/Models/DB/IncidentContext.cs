using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models.DB
{
    public class IncidentContext : DbContext
    {
        public IncidentContext(string conn)
            : base(conn)
        {
        }

        public virtual DbSet<IncidentModel> Incidents { get; set; }
    }
}