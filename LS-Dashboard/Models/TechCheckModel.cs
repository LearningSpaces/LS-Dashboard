using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class TechCheckModel
    {

        [JsonProperty("name")]
        public string  Name { get; set; }
        [JsonProperty("status")]
        public bool LocationStatus { get; set; }

    }
}