using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
using Microsoft.AspNet.SignalR.Hubs;
using LS_Dashboard.Models.DB;

// SignalR hub for handling ServiceNow/Cherwell Incidents
namespace LS_Dashboard.Hubs
{
    [HubName("IncidentHub")]
    public class IncidentHub : Hub
    {
        // Used for clients to update incidents with custom notes and availability
        [HubMethodName("NewIncidentUpdate")]
        public void NewIncidentUpdate(string number, string note, string availability)
        {
            // Open a connection to the database
            using (var db = DbFactory.getDb())
            {
                // Check if the database contains the incident
                if (!db.Incidents.Any(i => i.Number == number))
                {
                    // If not make a new incident object using the client's info and add it to the database
                    var Incident = new IncidentEntity()
                    {
                        Number = number,
                        Notes = note,
                        Availability = availability
                    };
                    db.Incidents.Add(Incident);
                }
                else
                {
                    // If it does, update the incident in the database
                    var Incident = db.Incidents.First(i => i.Number == number);
                    Incident.Notes = note;
                    Incident.Availability = availability;
                }
                // Save changes to the database
                db.SaveChanges();
            }

            // Send out the updated info to all connected clients
            Clients.All.updateIncident(number, note, availability);
        }

        // Method to get all of the incidents from ServiceNow/Cherwell
        [HubMethodName("GetIncidents")]
        public object GetIncidents(string table)
        {
            // Query strings for each of the tables
            const string ActionableQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^short_description>=~^ORcategory=Non-Case";
            const string FacultyAssistQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^subcategory=Faculty Assist";
            const string ImmediateAssistQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^u_class_in_session=Yes";
            
            var ActionableIncidents = new List<IncidentModel>();
            var FacultyAssistIncidents = new List<IncidentModel>();
            var ImmediateAssistIncidents = new List<IncidentModel>();

            // Get the results for each table from the ServiceNowHelper depending on the requested table
            switch (table)
            {
                case "actionable":
                    ActionableIncidents = ServiceNowHelper.getIncidents(ActionableQuery, false, 15);
                    break;
                case "faculty_assist":
                    FacultyAssistIncidents = ServiceNowHelper.getIncidents(FacultyAssistQuery, false, 15);
                    break;
                case "immediate_assist":
                    ImmediateAssistIncidents = ServiceNowHelper.getIncidents(ImmediateAssistQuery, false, 15);
                    break;
                default:
                    ActionableIncidents = ServiceNowHelper.getIncidents(ActionableQuery, false, 15);
                    FacultyAssistIncidents = ServiceNowHelper.getIncidents(FacultyAssistQuery, false, 15);
                    ImmediateAssistIncidents = ServiceNowHelper.getIncidents(ImmediateAssistQuery, false, 15);
                    break;
            }
            
            // Open the database to get any notes/availability
            using (var db = DbFactory.getDb())
            {
                foreach (var Inc in ActionableIncidents)
                {
                    var DBInc = db.Incidents.SingleOrDefault(i => i.Number == Inc.Number);
                    if (DBInc != null)
                    {
                        Inc.Availability = DBInc.Availability;
                        Inc.Notes = DBInc.Notes;
                    }
                    else
                    {
                        Inc.Availability = "";
                        Inc.Notes = "";
                    }
                }
                foreach (var Inc in FacultyAssistIncidents)
                {
                    var DBInc = db.Incidents.SingleOrDefault(i => i.Number == Inc.Number);
                    if (DBInc != null)
                    {
                        Inc.Availability = DBInc.Availability;
                        Inc.Notes = DBInc.Notes;
                    }
                    else
                    {
                        Inc.Availability = "";
                        Inc.Notes = "";
                    }
                }
                foreach (var Inc in ImmediateAssistIncidents)
                {
                    var DBInc = db.Incidents.SingleOrDefault(i => i.Number == Inc.Number);
                    if (DBInc != null)
                    {
                        Inc.Availability = DBInc.Availability;
                        Inc.Notes = DBInc.Notes;
                    }
                    else
                    {
                        Inc.Availability = "";
                        Inc.Notes = "";
                    }
                }
            }
            // Return an object with all of the results
            return new
            {
                actionable = ActionableIncidents,
                faculty_assist = FacultyAssistIncidents,
                immediate_assist = ImmediateAssistIncidents
            };
        }
    }
}