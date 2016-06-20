using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using LS_Dashboard.Helpers;
using LS_Dashboard.Models;
using Microsoft.AspNet.SignalR.Hubs;
using LS_Dashboard.Models.DB;

namespace LS_Dashboard.Hubs
{
    [HubName("IncidentHub")]
    public class IncidentHub : Hub
    {
        [HubMethodName("NewIncidentUpdate")]
        public void NewIncidentUpdate(string number, string note, string availability)
        {
            using (var db = DbFactory.getDb())
            {
                if (!db.Incidents.Any(i => i.Number == number))
                {
                    var Incident = new IncidentEntity()
                    {
                        Number = number,
                        Notes = note,
                        Availability = availability
                    };
                    db.Incidents.Add(Incident);
                    db.SaveChanges();
                }
                else
                {
                    var Incident = db.Incidents.First(i => i.Number == number);
                    Incident.Notes = note;
                    Incident.Availability = availability;
                    db.SaveChanges();
                }
            }

            Clients.All.updateIncident(number, note, availability);
        }

        [HubMethodName("GetIncidents")]
        public object GetIncidents()
        {
            const string ActionableQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^short_description>=~^ORcategory=Non-Case";
            const string FacultyAssistQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^subcategory=Faculty Assist";
            const string ImmediateAssistQuery = "assignment_group=560ec8c34a3623260149ea9979e736a2^ORassignment_group=560ec9344a36232601893503bc6721a8^incident_state<6^u_class_in_session=Yes";
            var ActionableIncidents = ServiceNowHelper.getRecords(ServiceNowHelper.Table.Incident, ActionableQuery, false, 15);
            var FacultyAssistIncidents = ServiceNowHelper.getRecords(ServiceNowHelper.Table.Incident, FacultyAssistQuery, false, 15);
            var ImmediateAssistIncidents = ServiceNowHelper.getRecords(ServiceNowHelper.Table.Incident, ImmediateAssistQuery, false, 15);
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
            }
            return new
            {
                actionable = ActionableIncidents,
                faculty_assist = FacultyAssistIncidents,
                immediate_assist = ImmediateAssistIncidents
            };
        }

        public object GetShifts()
        {
            int CurrentShift = WhenIWorkHelper.GetShiftFromTime(DateTime.Now);
            var CurrentShifts = WhenIWorkHelper.GetShifts(CurrentShift);
            var NextShifts = WhenIWorkHelper.GetShifts(CurrentShift + 1);

            return new
            {
                current = new
                {
                    shifts = CurrentShifts,
                    number = CurrentShift
                },
                next = new
                {
                    shifts = NextShifts,
                    number = CurrentShift + 1
                }
            };
        }

        public object GetTechChecks()
        {
            var TechChecks = GoogleDriveHelper.GetTechChecks();
            return new
            {
                sectors = TechChecks
            };
        }

        public object GetVehicle()
        {
            var Vehicles = GoogleDriveHelper.GetVehicles();
            return new
            {
                vehicles = Vehicles
            };
        }
    }
}