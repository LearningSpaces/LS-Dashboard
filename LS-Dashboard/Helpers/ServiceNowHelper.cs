using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace LS_Dashboard.Helpers
{
    public class ServiceNowHelper
    {
        //Enum for the ServiceNow tables to search
        public enum Table
        {
            Incident,
            User,
            Building,
            Room
        }

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
        public static object getRecords(Table table, string query = "active=true", bool displayValues = false, int recordCount = -1)
        {
            string tableString = "";
            //Change table selection to the string representing the table in ServiceNow
            switch (table)
            {
                case Table.Building:
                    tableString = "cmn_building";
                    break;
                case Table.Incident:
                    tableString = "incident_list";
                    break;
                case Table.Room:
                    tableString = "room_location";
                    break;
                case Table.User:
                    tableString = "sys_user";
                    break;
            }
            if (tableString == "")
            {
                return @"{""error"":
                    ""Table not recognized""
                }";
            }
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
                        var serializer = new JavaScriptSerializer();
                        return serializer.DeserializeObject(json);
                    }

                }
                catch (Exception e)
                {
                    //Something bad happened. Log Results
                    //ErrorLogger.medLog(e.ToString());
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //ErrorLogger.medLog(e.ToString());
            }

            return @"{""error"":
                    ""Unknown Error""
                }";
        }
    }
}