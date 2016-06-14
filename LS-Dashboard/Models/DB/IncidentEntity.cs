using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models.DB
{
    [Table("Incidents")]
    public class IncidentEntity
    {
        [Key]
        [Column("IncidentNumber")]
        public string Number { get; set; }
        public string Notes { get; set; }
        public string Availability { get; set; }
    }
}