using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class CartModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("checkout_info")]
        public CheckoutData CheckoutInfo { get; set; }
        [JsonProperty("status")]
        public string CartStatus { get; set; }

        public class CheckoutData
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("number")]
            public string Number { get; set; }
            [JsonProperty("section")]
            public string Section { get; set; }
        }

        public enum Status
        {
            Store,
            CheckIn,
            CheckOut
        }
    }
}