using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class WiWLoginModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("id")]
        public int AccountId { get; set; }

    }
}