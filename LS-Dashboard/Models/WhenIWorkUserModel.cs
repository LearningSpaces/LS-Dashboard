using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class WhenIWorkUserModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("first_name")]
        public string FirstName { get; set; }
        [JsonProperty("last_name")]
        public string LastName { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}