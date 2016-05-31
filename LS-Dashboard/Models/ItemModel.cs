using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class ItemModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("checkout_info")]
        public CheckoutData CheckoutInfo { get; set; }
        [JsonProperty("status")]
        public string ItemStatus { get; set; }

        public class CheckoutData
        {
            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public enum Status
        {
            CheckIn,
            CheckOut
        }
    }
}