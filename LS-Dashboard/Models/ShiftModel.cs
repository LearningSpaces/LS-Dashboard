using LS_Dashboard.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    public class ShiftModel
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("account_id")]
        public int AccountID { get; set; }
        [JsonProperty("user_id")]
        public int UserID { get; set; }
        [JsonProperty("location_id")]
        public int LocationID { get; set; }
        [JsonProperty("position_id")]
        public int PositionID { get; set; }
        [JsonProperty("site_id")]
        public int SiteID { get; set; }
        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }
        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("instances")]
        public int Instances { get; set; }
        [JsonProperty("is_open")]
        public bool IsOpen { get; set; }

        public WhenIWorkUserModel User
        {
            get
            {
                return WhenIWorkHelper.GetUser(UserID);
            }
        }
    }
}