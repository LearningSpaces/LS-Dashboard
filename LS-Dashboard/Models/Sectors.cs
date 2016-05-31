using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class TechCheckSector
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("labs")]
        public List<TechCheckModel> Labs { get; set; }

        public int Total
        {
            get
            {
                return Labs.Count;
            }
        }

        public int Remaining
        {
            get
            {
                return Labs.Count(lab => !lab.LocationStatus);
            }
        }

        public int Completed
        {
            get
            {
                return Labs.Count(lab => lab.LocationStatus);
            }
        }

    }
}
