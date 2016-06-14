using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual DbSet<IncidentEntity> Incidents { get; set; }
    }
}