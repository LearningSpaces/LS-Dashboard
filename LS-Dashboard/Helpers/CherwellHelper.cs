using LS_Dashboard.Cherwell.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace LS_Dashboard.Helpers
{
    public static class CherwellHelper
    {
        private const string IncidentId = "9355d5ed41e384ff345b014b6cb1c6e748594aea5b";
        private const string TaskId = "6dd53665c0c24cab86870a21cf6434ae";
        private const string UserId = "94162d044759c7c8a2236f4e7297da7d914c5a5fd1";
        private const string UserName = @"CHERWELL\ITLearningSpacesSA";
        private const string Password = "sendinthewaxers";
        private const string ApiKey = "4cd3cbb8-481d-4254-abca-ae21b36a934c";
        private const string BaseUrl = @"https://ou.cherwellondemand/CherwellAPI";
        private static TokenResponse Token = null;

        private static bool Login()
        {
            var request = WebRequest.CreateHttp(BaseUrl + @"/token");
            var Data = "grant_type=password&client_id=" + ApiKey +"&username=" + UserName + "&password=" + Password;
            var bytes = Encoding.UTF8.GetBytes(Data);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<TokenResponse>(json);
                    if (results.Error != null)
                    {
                        Token = null;
                    }
                    else
                    {
                        Token = results;
                    }
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }
            return Token != null;
        }

        private static bool Refresh()
        {
            if (Token == null)
            {
                return Login();
            }
            var request = WebRequest.CreateHttp(BaseUrl + @"/token");
            var Data = "grant_type=refresh_token&client_id=" + ApiKey + "&refresh_token=" + Token.RefreshToken;
            var bytes = Encoding.UTF8.GetBytes(Data);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "application/json";
            request.ContentLength = bytes.Length;
            var stream = request.GetRequestStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string json = reader.ReadToEnd();
                    var results = JsonConvert.DeserializeObject<TokenResponse>(json);
                    if (results.Error != null)
                    {
                        Token = null;
                    }
                    else
                    {
                        Token = results;
                    }
                }
            }
            catch (Exception e)
            {
                //Something bad happened. Log Results
                //Logger.Med(e.ToString());
            }
            return Token != null;
        }

        private static bool CheckLogin()
        {
            if (Token == null || Token.Expires < DateTime.Now)
            {
                return Login();
            }
            else if(Token.Expires < DateTime.Now.AddMinutes(5))
            {
                return true;
            }

            return false;
        }

        private static bool Logout()
        {
            return true;
        }

        public static SearchResultsModel getTasks()
        {
            return new SearchResultsModel();
        }
    }
}