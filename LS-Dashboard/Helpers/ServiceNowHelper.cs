using LS_Dashboard.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace LS_Dashboard.Helpers
{
    public static class ServiceNowHelper
    {
        //ServiceNow instance url
        private static string ServiceNowUrl = "https://ounew.service-now.com";
        //ServiceNow user credentials
        private static NetworkCredential cred = new NetworkCredential("queuedisp", "display2015");
        //Use cookies to keep from breaking ServiceNow
        private static CookieContainer cookieJar = new CookieContainer();

        private static CredentialCache credCache = new CredentialCache();


        //Method for fetching data from ServiceNow
        //  table: table to fetch data from
        //  query: ServiceNow WebRequest API query. See ServiceNow Wiki for info
        //  displayValues: boolean for whether or not to display values for fields that would normally show a reference number
        //    displayValues = true is slower but contains more detailed information
        //  recordCount: limits the number of returned records to a certain amount
        public static List<IncidentModel> getIncidents(string query = "active=true", bool displayValues = false, int recordCount = -1)
        {
            string tableString = "incident_list";

            if (credCache.GetCredential(new Uri(ServiceNowUrl), "Basic") == null)
            {
                credCache.Add(new Uri(ServiceNowUrl), "Basic", cred);
            }
            //Try building the ServiceNow get request string. Then make the request and parse/return the response
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        System.Diagnostics.Debug.WriteLine(sender);
                        return true;
                    });
                HttpWebRequest request = WebRequest.CreateHttp(ServiceNowUrl + "/" + tableString + ".do?JSONv2&displayvalue=" + (displayValues ? "all" : "false") + "&sysparm_record_count=" + recordCount + "&sysparm_action=getRecords&sysparm_query=" + query);
                request.Accept = "application/json";
                request.Credentials = credCache;
                request.CookieContainer = cookieJar;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string json = reader.ReadToEnd();
                        var results = JObject.Parse(json);
                        return results["records"].ToObject<List<IncidentModel>>();
                    }

                }
                catch (Exception e)
                {
                    //Something bad happened. Log Results
                    //Logger.Med(e.ToString());
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }

            return new List<IncidentModel>();
        }

        public static List<IncidentModel> getRooms(string query = "active=true", int recordCount = -1)
        {
            string tableString = "room_location";

            if (credCache.GetCredential(new Uri(ServiceNowUrl), "Basic") == null)
            {
                credCache.Add(new Uri(ServiceNowUrl), "Basic", cred);
            }
            //Try building the ServiceNow get request string. Then make the request and parse/return the response
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        System.Diagnostics.Debug.WriteLine(sender);
                        return true;
                    });
                HttpWebRequest request = WebRequest.CreateHttp(ServiceNowUrl + "/" + tableString + ".do?JSONv2&sysparm_record_count=" + recordCount + "&sysparm_action=getRecords&sysparm_query=" + query);
                request.Accept = "application/json";
                request.Credentials = credCache;
                request.CookieContainer = cookieJar;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string json = reader.ReadToEnd();
                        var results = JObject.Parse(json);
                        return results["records"].ToObject<List<IncidentModel>>();
                    }

                }
                catch (Exception e)
                {
                    //Something bad happened. Log Results
                    //Logger.Med(e.ToString());
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }

            return new List<IncidentModel>();
        }

        public static List<IncidentModel> getBuildings(string query = "active=true", int recordCount = -1)
        {
            string tableString = "cmn_building";

            if (credCache.GetCredential(new Uri(ServiceNowUrl), "Basic") == null)
            {
                credCache.Add(new Uri(ServiceNowUrl), "Basic", cred);
            }
            //Try building the ServiceNow get request string. Then make the request and parse/return the response
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        System.Diagnostics.Debug.WriteLine(sender);
                        return true;
                    });
                HttpWebRequest request = WebRequest.CreateHttp(ServiceNowUrl + "/" + tableString + ".do?JSONv2&sysparm_record_count=" + recordCount + "&sysparm_action=getRecords&sysparm_query=" + query);
                request.Accept = "application/json";
                request.Credentials = credCache;
                request.CookieContainer = cookieJar;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string json = reader.ReadToEnd();
                        var results = JObject.Parse(json);
                        return results["records"].ToObject<List<IncidentModel>>();
                    }

                }
                catch (Exception e)
                {
                    //Something bad happened. Log Results
                    //Logger.Med(e.ToString());
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }

            return new List<IncidentModel>();
        }

        public static List<IncidentModel> getUsers(string query = "active=true", int recordCount = -1)
        {
            string tableString = "sys_user";

            if (credCache.GetCredential(new Uri(ServiceNowUrl), "Basic") == null)
            {
                credCache.Add(new Uri(ServiceNowUrl), "Basic", cred);
            }
            //Try building the ServiceNow get request string. Then make the request and parse/return the response
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = (
                    (sender, certificate, chain, sslPolicyErrors) =>
                    {
                        System.Diagnostics.Debug.WriteLine(sender);
                        return true;
                    });
                HttpWebRequest request = WebRequest.CreateHttp(ServiceNowUrl + "/" + tableString + ".do?JSONv2&sysparm_record_count=" + recordCount + "&sysparm_action=getRecords&sysparm_query=" + query);
                request.Accept = "application/json";
                request.Credentials = credCache;
                request.CookieContainer = cookieJar;
                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string json = reader.ReadToEnd();
                        var results = JObject.Parse(json);
                        return results["records"].ToObject<List<IncidentModel>>();
                    }

                }
                catch (Exception e)
                {
                    //Something bad happened. Log Results
                    //Logger.Med(e.ToString());
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }

            return new List<IncidentModel>();
        }
    }
}