﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LS_Dashboard.Models
{
    [Table("Incidents")]
    public class IncidentModel
    {
        //ServiceNow Native Fields
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("u_affected_user")]
        public string AffectUser { get; set; }
        [JsonProperty("sys_created_by")]
        public string CreatedBy { get; set; }
        [JsonProperty("priority")]
        public int Priority { get; set; }
        [JsonProperty("sys_updated_on")]
        public DateTime UpdatedOn { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
        [JsonProperty("assignment_group")]
        public string AssignmentGroup { get; set; }
        [JsonProperty("state")]
        public int State { get; set; }
        [JsonProperty("sys_created_on")]
        public DateTime CreatedOn { get; set; }
        [JsonProperty("opened_at")]
        public DateTime OpenedOn { get; set; }
        [JsonProperty("sys_updated_by")]
        public string UpdatedBy { get; set; }
        [JsonProperty("assigned_to")]
        public string AssignedTo { get; set; }
        [JsonProperty("opened_by")]
        public string OpenedBy { get; set; }
        [JsonProperty("short_description")]
        public string ShortDescription { get; set; }

        //Added Dashboard Fields
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("availability")]
        public string Availability { get; set; }
        [JsonProperty("regent_code")]
        public string RegentCode { get; set; }
        [JsonProperty("room")]
        public string Room { get; set; }
        [JsonProperty("combined_loc")]
        public string CombinedLocation
        {
            get
            {
                return RegentCode + " " + Room;
            }
        }
    }
}